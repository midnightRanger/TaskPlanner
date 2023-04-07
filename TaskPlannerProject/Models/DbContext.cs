using Microsoft.EntityFrameworkCore;

namespace TaskPlannerProject.Models;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbContext()
        {
            
        }
        
        public DbSet<Task> Task { get; set; } = null!;
        public DbSet<Goal> Goal { get; set; } = null!;
        public DbSet<Tag> Tag { get; set; } = null!;
        public DbSet<Task> User { get; set; } = null!;
        public DbSet<Role> Role { get; set; } = null!; 
}