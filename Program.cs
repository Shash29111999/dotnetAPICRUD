//using TodoAPI.AppDataContext;
//using TodoAPI.Middleware;
//using TodoAPI.Interface;
//using TodoAPI.Services;
//using Microsoft.EntityFrameworkCore;
//using TodoAPICS.Interfaces;
//using TodoAPICS.Services;
//using System.Diagnostics;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//// Register DbContext with SQL Server
//builder.Services.AddDbContext<TodoDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 
//builder.Services.AddProblemDetails();

//// Add Logging 
//builder.Services.AddLogging(); 
//builder.Services.AddScoped<ITodoServices, TodoServices>();
//builder.Services.AddScoped<IUsersService, UsersServices>();



//var app = builder.Build();

//// Apply database migrations automatically
//using (var scope = app.Services.CreateScope()) 
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
//    dbContext.Database.Migrate(); // Applies any pending migrations
//}

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//app.Use(async (context, next) =>
//{
//    var stopwatch = Stopwatch.StartNew();
//    Console.WriteLine($"[Request] --> {context.Request.Method} {context.Request.Path}");

//    // Pass the request to the next middleware in the pipeline
//    await next(context);

//    // This code executes AFTER the rest of the pipeline has processed the request
//    stopwatch.Stop();
//    Console.WriteLine($"[Response] ---> {context.Response.StatusCode} for {context.Request.Path} in {stopwatch.ElapsedMilliseconds}ms");
//});

////app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using TodoAPI.AppDataContext;
using TodoAPI.Middleware;
using TodoAPI.Interface;
using TodoAPI.Services;
using Microsoft.EntityFrameworkCore;
using TodoAPICS.Interfaces;
using TodoAPICS.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TodoAPICS.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext with SQL Server
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<UsersDetails, Role>()
    .AddEntityFrameworkStores<TodoDbContext>() // Use EF Core as the store for Identity
    .AddDefaultTokenProviders();  // For password reset tokens, etc.

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configure Authorization policies (Optional, but good for fine-grained control)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ViewerPolicy", policy => policy.RequireRole("Viewer"));
    // You can also define policies for combinations of roles or claims
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register the custom exception handler service
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Required for UseExceptionHandler

// Add Logging
builder.Services.AddLogging();
builder.Services.AddScoped<ITodoServices, TodoServices>();
builder.Services.AddScoped<IUsersService, UsersServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();

// Add CORS services (configure policies later if needed)
builder.Services.AddCors(options =>
{
    Console.WriteLine($"[Inside Cors configuaration]");

   options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin() // In production, restrict this to specific origins
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add Authentication services (e.g., JWT Bearer, Cookie, etc.)
// You would configure specific authentication schemes here.
// For example: builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(...);
builder.Services.AddAuthentication();


var app = builder.Build();

// Apply database migrations automatically (This is application startup logic, not middleware)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    dbContext.Database.Migrate(); // Applies any pending migrations
}

// =========================================================
// Configure the HTTP request pipeline. (Middleware Order Matters!)
// =========================================================

// Middleware 1: Exception Handling (should be very early to catch errors from subsequent middleware)
// This uses the GlobalExceptionHandler registered in services
app.UseExceptionHandler();

// Middleware 2: Development-specific tools (Swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Uncomment in production for HTTPS enforcement

// Middleware 3: Custom Logging/Timing Middleware (Inline Example)
// This middleware logs request details and response time.
app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");

    // Pass the request to the next middleware in the pipeline
    await next(context);

    // This code executes AFTER the rest of the pipeline has processed the request
    stopwatch.Stop();
    Console.WriteLine($"[Response] {context.Response.StatusCode} for {context.Request.Path} in {stopwatch.ElapsedMilliseconds}ms");
});

// Middleware 4: Routing (Matches the request to an endpoint)
// This should be before Authentication, Authorization, and Endpoint mapping.
app.UseRouting();

// Middleware 5: CORS (Cross-Origin Resource Sharing)
// Place after UseRouting and before UseAuthentication/UseAuthorization
app.UseCors(); // Uses the default policy configured above

// Middleware 6: Authentication (Identifies the user based on credentials)
// Place after UseRouting and UseCors, but before UseAuthorization
app.UseAuthentication();

// Middleware 7: Authorization (Checks if the identified user has permission to access the endpoint)
// Place after UseAuthentication
app.UseAuthorization();

// Middleware 8: Endpoint Mapping (Executes the actual controller action)
// This is a terminal middleware for routing to controllers.
// It effectively marks the end of the pipeline for successful requests.
app.MapControllers();

// Middleware 9: Application Run (Starts the web host)
app.Run();

