using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskPlannerProject.Models.ViewModels;

public class UpdatePlanViewModel
{
    public List<Plan> Plans { get; set; } = new();
    public string? Title { get; set; }
    public string? Color { get; set; }
    public string? Reason { get; set; }
    
    public int ListId { get; set; }
    public SelectList AllLists { get; set; }
    public PlanList PlanList { get; set; }
    
    public int UpdatingId { get; set; }
    
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}