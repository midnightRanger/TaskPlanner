using Microsoft.AspNetCore.Mvc;
using TaskPlannerProject.Models;
using Microsoft.EntityFrameworkCore;
using TaskPlannerProject.Models.ViewModels;
using DbContext = TaskPlannerProject.Models.DbContext;

namespace TaskPlannerProject.Controllers;

public class CalendarController : Controller
{
    private readonly DbContext _db;

    public CalendarController(DbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Calendar()
    {
        var author = await _db.User.Where(u => u.Login == User.Identity.Name).SingleOrDefaultAsync();

        List<Plan> plans = await _db.Plan.Where(p => p.UserId == author.Id).ToListAsync();
        List<EventViewModel> events = new();

        foreach (var plan in plans)
            events.Add(new EventViewModel() { desc = plan.Title, 
                startDateDay = plan.Start.Day, endDateDay = plan.End.Day, 
                endDateMonth = plan.End.Month-1, startDateMonth = plan.Start.Month-1,
                endDateYear = plan.End.Year, startDateYear = plan.Start.Year
            });
        CalendarViewModel calendarViewModel = new CalendarViewModel()
        {
            events = events
        }; 

        return View(calendarViewModel);
    }
}