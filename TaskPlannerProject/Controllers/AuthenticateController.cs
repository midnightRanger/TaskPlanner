using Microsoft.AspNetCore.Mvc;

namespace TaskPlannerProject.Controllers;

public class AuthenticateController: Controller
{

    public async Task<IActionResult> Login()
    {
        return View(); 
    }
}