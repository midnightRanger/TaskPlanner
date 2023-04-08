using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskPlannerProject.Models.ViewModels;

public class UpdatePlanViewModel
{
    public List<Plan> Plans { get; set; } = new();
    public string? Title { get; set; }
    public string? Color { get; set; }
    
    public int ListId { get; set; }
    public SelectList AllLists { get; set; }
    public PlanList PlanList { get; set; }
    
    public int UpdatingId { get; set; }
}