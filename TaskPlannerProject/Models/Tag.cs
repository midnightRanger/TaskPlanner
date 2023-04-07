using System.ComponentModel.DataAnnotations;

namespace TaskPlannerProject.Models;

public class Tag
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TaskId { get; set; }
    public Task? Task { get; set; }
}