using Microsoft.AspNetCore.Mvc;
using TaskPlannerProject.Models;
using TaskPlannerProject.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using DbContext = TaskPlannerProject.Models.DbContext;

namespace TaskPlannerProject.Controllers;

public class PlanController: Controller
{
    private readonly DbContext _db;

    public PlanController(DbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? message)
    {
        if (message != null)
            ModelState.AddModelError("", message);

        var author = await _db.User.Where(u => u.Login == User.Identity.Name).SingleOrDefaultAsync();
        List<Plan> plans = await _db.Plan.Where(p => p.UserId == author.Id).ToListAsync();

        UpdatePlanViewModel model = new UpdatePlanViewModel();
        model.Plans = plans;

        var lists = from PlanList p in Enum.GetValues(typeof(PlanList))
            select new { ID = (int)p, Name = p.ToString() };

        model.AllLists = new(lists, "ID", "Name");


        return View(model);
    }

}