using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.AppDataContext
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<Todo> TodoAPI { get; set; } // Ensure the table is named correctly
        public DbSet<User> UserAPI { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .ToTable("TodoAPI")  // Fix table name
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
              .ToTable("Users")  // Fix table name
              .HasKey(x => x.Id);
        }
    }
}
