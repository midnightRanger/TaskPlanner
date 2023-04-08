
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
    
    public async Task<IActionResult> DeletePlan(int? id)
    {
        try
        {
            Plan plan = await _db.Plan.Where(t => t.Id == id).SingleOrDefaultAsync();
            _db.Plan.Remove(plan);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return PartialView("Toastr", new Response() { Description = ex.Message });
        }
        
        return PartialView("Toastr", new Response() { Description = "Plan was deleted" });

    }
    
    public async Task<IActionResult> AddPlan(string? Title)
    {
        User user = await _db.User.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);
        if (Title == null) 
            return RedirectToAction("PlanIndex", "Plan", new { message = "Fill the Title field!" });

        var model = new Plan()
        {
            Title = Title, Color = "#ABBAEA", UserId = user.Id, PlanList = PlanList.New, 
            Start = DateTime.Now, End = DateTime.Now
        };
        try
        {
            await _db.Plan.AddAsync(model);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return RedirectToAction("PlanIndex", "Plan", new { message = ex.Message });
        }

        return RedirectToAction("PlanIndex", "Plan");
    }
    
    [HttpPost]
    public async Task<IActionResult> PlanUpdate(int id, UpdatePlanViewModel model)
    {
        if (model.Title == null)
            return PartialView("Toastr", new Response() { Description = "Fill the title field!" });
        var planToUpdate = await _db.Plan.FirstOrDefaultAsync(t => t.Id == id);

        if (planToUpdate == null)
            return PartialView("Toastr", new Response() { Description = "Internal server error" });
        planToUpdate.Title = model.Title;
        planToUpdate.PlanList = (PlanList)model.ListId;
        planToUpdate.Start = model.Start;
        planToUpdate.End = model.End;
        planToUpdate.Reason = model.Reason; 

        try
        {
            _db.Plan.Update(planToUpdate);
            _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return PartialView("Toastr", new Response() { Description = ex.Message });
        }


        return PartialView("Toastr", new Response() { Description = "Plan was updated" });
    }

    public async Task<IActionResult> PlanIndex(string? message)
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