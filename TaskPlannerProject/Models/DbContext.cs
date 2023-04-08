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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(new[]
        {
            new Role() { Id = 1, Name = "ADMIN" },
            new Role() { Id = 2, Name = "User" },
        });

        modelBuilder.Entity<User>().HasData(new[]
            {
                new User()
                {
                    Id = 1, Email = "admin@admin.com", Login = "admin",
                    Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"
                }
            }
        );

        modelBuilder.Entity<Task>().HasData(new[]
        {
            new Task()
            {
                Id = 1, Color = "#ABBAEA", Title = "Wash car", UserId = 1,
                TaskList = TaskList.Deligated, TaskPriority = TaskPriority.LOW
            },
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Task> Task { get; set; } = null!;
    public DbSet<Goal> Goal { get; set; } = null!;
    public DbSet<Tag> Tag { get; set; } = null!;
    public DbSet<Plan> Plan { get; set; } = null!; 
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;
}