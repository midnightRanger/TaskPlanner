namespace TaskPlannerProject.Models.ViewModels;

public class EventViewModel
{
    public string? desc { get; set; }
    public int startDateYear { get; set; }
    public int startDateMonth { get; set; }
    public int startDateDay { get; set; }
    
    public int endDateYear { get; set; }
    public int endDateMonth { get; set; }
    public int endDateDay { get; set; }
    
    public string? start {get;set;}
    public string? end {get;set;}
}

public class CustomDate
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}