using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskPlannerProject.Models.ViewModels;

public class UpdateTaskViewModel
{
    public List<Task> Tasks { get; set; } = new();
    public string? Title { get; set; }
    public string? Color { get; set; }
    public int PriorityId { get; set; }
    public SelectList AllPriorities { get; set; }
    
    public int ListId { get; set; }
    public SelectList AllLists { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public TaskList TaskList { get; set; }
    
    public int UpdatingId { get; set; }
}