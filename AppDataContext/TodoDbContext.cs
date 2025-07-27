using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPICS.Models;



namespace TodoAPI.AppDataContext
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<Todo> TodoAPI { get; set; } // Ensure the table is named correctly
        public DbSet<User> UserAPI { get; set; }

        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .ToTable("TodoAPI")  // Fix table name
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
              .ToTable("Users")  // Fix table name
              .HasKey(x => x.Id);

            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(x => x.Id);

            // Explicitly define composite primary key for IdentityUserRole<string>
            // This is often needed when seeding data or if EF Core's conventions
            // aren't picking it up automatically in specific scenarios.
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = "admin-role-id", Name = "Admin", NormalizedName = "ADMIN" },
               new Role { Id = "viewer-role-id", Name = "Viewer", NormalizedName = "VIEWER" }
           );

            // Seed a test Admin User
            //var adminUser = new UsersDetails
            //{
            //    Id = "admin-user-id",
            //    UserName = "adminuser",
            //    NormalizedUserName = "ADMINUSER",
            //    Email = "admin@example.com",
            //    NormalizedEmail = "ADMIN@EXAMPLE.COM",
            //    EmailConfirmed = true,
            //    SecurityStamp = "602a5976-95eb-4c60-83d9-abe67c0897d2", // Important for Identity
            //    PasswordHash = "AQAAAAIAAYagAAAAEK9sBhuBcmpIJlgZx071ur2tuHPb18tRqP0tTbq/MQyi9X3q7kzzuRRzWBbl5PMV5Q==" // ← Pre-hashed
            //};

            // Hash the password for the admin user
            //var passwordHasher = new PasswordHasher<UsersDetails>();
            //adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123"); // CHANGE THIS PASSWORD!

            modelBuilder.Entity<UsersDetails>()
                .ToTable("UsersDetails")
                .HasKey(x => x.Id);

            // Link Admin User to Admin Role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "admin-role-id",
                    UserId = "admin-user-id"
                }
            );
        }
    }
}
