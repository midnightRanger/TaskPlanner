using Microsoft.AspNetCore.Mvc;
using TaskPlannerProject.Models;
using TaskPlannerProject.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using DbContext = TaskPlannerProject.Models.DbContext;

namespace TaskPlannerProject.Controllers;

public class AuthenticateController: Controller
{
    private readonly DbContext _db;

    public AuthenticateController(DbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Login(string? message)
    {
        if (message != null)
            ModelState.AddModelError("",message);
        return View(); 
    }

    public async Task<IActionResult> Login(AuthViewModel model)
    {
        if (model.Login == null || model.Password == null)
            return RedirectToAction("Login", "Authenticate", new { message = "One or more fields are not filled!"});
        try
        {
            var user = await _db.User.Where(u=>u.Login == model.Login).SingleOrDefaultAsync();
            
            if (user == null) 
                return RedirectToAction("Login", "Authenticate", new { message = "User not found!"});
            
            if (user.Password != model.Password)
                return RedirectToAction("Login", "Authenticate", new { message = "Wrong password!"});

        }
        catch (Exception ex)
        {
            return RedirectToAction("Login", "Authenticate", new { message = ex.Message});
        }

    }
}