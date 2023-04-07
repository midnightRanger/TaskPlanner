using System.ComponentModel.DataAnnotations;

namespace TaskPlannerProject.Models;

public class Task
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Color { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public TaskList TaskList { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; } = new(); 
    
    public List<Tag> Tags { get; set; } = new();
    
}

public enum TaskPriority
{
    CRITICAL,
    HIGH,
    MID, 
    LOW
}

public enum TaskList
{
    New,
    ToDo,
    Deligated,
    Completed
}
