using System.ComponentModel.DataAnnotations;

namespace TaskPlannerProject.Models;

public class Goal
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public string? Color { get; set; }
    public GoalList GoalList { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; } = new();
}

public enum GoalList
{
    ToRelease, 
    Later, 
    New,
    Completed
}