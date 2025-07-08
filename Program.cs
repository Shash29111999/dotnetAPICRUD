using TodoAPI.AppDataContext;
using TodoAPI.Middleware;
using TodoAPI.Interface;
using TodoAPI.Services;
using Microsoft.EntityFrameworkCore;
using TodoAPICS.Interfaces;
using TodoAPICS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext with SQL Server
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 
builder.Services.AddProblemDetails();

// Add Logging 
builder.Services.AddLogging(); 
builder.Services.AddScoped<ITodoServices, TodoServices>();
builder.Services.AddScoped<IUsersService, UsersServices>();



var app = builder.Build();

// Apply database migrations automatically
using (var scope = app.Services.CreateScope()) 
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    dbContext.Database.Migrate(); // Applies any pending migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
