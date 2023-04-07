using Microsoft.AspNetCore.Mvc;
using TaskPlannerProject.Models;
using TaskPlannerProject.Models.ViewModels;

namespace TaskPlannerProject.Controllers;

public class AuthenticateController: Controller
{
    private readonly DbContext _db;

    public AuthenticateController(DbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Login()
    {
        return View(); 
    }

    public async Task<IActionResult> Login(AuthViewModel model)
    {
        
    }
}