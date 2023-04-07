using System.ComponentModel.DataAnnotations;

namespace TaskPlannerProject.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public List<Goal> Goals { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();
    public List<Role> Roles { get; set; } = new(); 
}