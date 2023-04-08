using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskPlannerProject.Models;
using Microsoft.EntityFrameworkCore;
using TaskPlannerProject.Models.ViewModels;
using DbContext = TaskPlannerProject.Models.DbContext;
using Task = TaskPlannerProject.Models.Task;

namespace TaskPlannerProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DbContext _db;

    public HomeController(ILogger<HomeController> logger, DbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> DeleteTask(int? id)
    {
        try
        {
            Task task = await _db.Task.Where(t => t.Id == id).SingleOrDefaultAsync();
            _db.Task.Remove(task);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return PartialView("Toastr", new Response() {Description = ex.Message, Data = ex.Data.ToString()});
        }

        return PartialView("Toastr", new Response() {Description = "Task was deleted", Data = ""});
    }

    public async Task<IActionResult> AddTask(string? Title)
    {
        User user = await _db.User.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);
        if (Title == null)
            return RedirectToAction("Index", "Home", new { message = "Fill the Title field!" });

        var model = new Task()
        {
            Title = Title, Color = "#ABBAEA", UserId = user.Id, TaskList = TaskList.New, TaskPriority = TaskPriority.MID
        };
        try
        {
            await _db.Task.AddAsync(model);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return RedirectToAction("Index", "Home", new { message = ex.Message });
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Authenticate");
    }

    public async Task<IActionResult> Index(string? message, Task.SortState sortTask = Task.SortState.TitleAsc)
    {
        if (message != null)
            ModelState.AddModelError("", message);

        var author = await _db.User.Where(u => u.Login == User.Identity.Name).SingleOrDefaultAsync();

        ViewData["IdSort"] = sortTask == Task.SortState.TitleAsc ? Task.SortState.TitleDesc : Task.SortState.TitleAsc;


        List<Task> tasks = await _db.Task.Where(t => t.UserId == author.Id).ToListAsync();

        switch (sortTask)
        {
            case Task.SortState.TitleAsc:
                tasks = tasks.OrderBy(t => t.Title).ToList();
                break;
            case Task.SortState.TitleDesc:
                tasks = tasks.OrderByDescending(t => t.Title).ToList();
                break;
        }


        UpdateTaskViewModel model = new UpdateTaskViewModel();
        model.Tasks = tasks;

        var priorities = from TaskPriority p in Enum.GetValues(typeof(TaskPriority))
            select new { ID = (int)p, Name = p.ToString() };
        var lists = from TaskList p in Enum.GetValues(typeof(TaskList))
            select new { ID = (int)p, Name = p.ToString() };

        model.AllPriorities = new(priorities, "ID", "Name");
        model.AllLists = new(lists, "ID", "Name");


        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> TaskUpdate(int id, UpdateTaskViewModel model)
    {
        if (model.Title == null)
            return PartialView("Toastr", new Response() {Description = "Empty Title field!", Data = ""});
        var taskToUpdate = await _db.Task.FirstOrDefaultAsync(t => t.Id == id);

        if (taskToUpdate == null)
            return PartialView("Toastr", new Response() {Description = "Internal server error!", Data = ""});
        
        taskToUpdate.Title = model.Title;
        taskToUpdate.TaskList = (TaskList)model.ListId;
        taskToUpdate.TaskPriority = (TaskPriority)model.PriorityId;

        switch (taskToUpdate.TaskPriority)
        {
            case TaskPriority.LOW:
                taskToUpdate.Color = "#2CE453";
                break;
            case TaskPriority.MID:
                taskToUpdate.Color = "#D5FC14";
                break;
            case TaskPriority.HIGH:
                taskToUpdate.Color = "#F48727";
                break;
            case TaskPriority.CRITICAL:
                taskToUpdate.Color = "#F7A3A3";
                break;
        }

        try
        {
            _db.Task.Update(taskToUpdate);
            _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return PartialView("Toastr", new Response() {Description = ex.Message, Data = ex.Data.ToString()});
        }


        return PartialView("Toastr", new Response() {Description = "Task was updated!", Data = ""});
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}