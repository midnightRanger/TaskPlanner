using System.ComponentModel.DataAnnotations;

namespace TaskPlannerProject.Models;

public class Plan
{
    [Key]
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    public string? Reason { get; set; }
    public string? Title { get; set; }
    public string? Color { get; set; }
    public PlanList PlanList { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}

public enum PlanList
{
    New,
    ToRelease,
    Later,
    Completed
}