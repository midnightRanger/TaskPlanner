using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskPlannerProject.Models;
using TaskPlannerProject.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using TaskPlannerProject.Helper;
using DbContext = TaskPlannerProject.Models.DbContext;

namespace TaskPlannerProject.Controllers;

public class AuthenticateController: Controller
{
    private readonly DbContext _db;

    public AuthenticateController(DbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<IActionResult> Register(string? message)
    {
        if (message != null)
            ModelState.AddModelError("",message);
        return View(); 
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (await _db.User.AnyAsync(u => u.Email == model.Email || u.Login == model.Login))
            return RedirectToAction("Register", "Authenticate",
                new { message = "User with following email or login is already exist" });
        List<Role> roles = new List<Role>();
        Role? role = await _db.Role.FirstAsync();

        User user = new User()
        {
            Email = model.Email,
            Password = Hasher.HashPassword(model.Password),
            Login = model.Login
        };

        try
        {
            await _db.User.AddAsync(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Login", "Authenticate", new { message = "User was successfully registered" }); 
        }
        catch (Exception ex)
        {
            return RedirectToAction("Register", "Authenticate", new { message = ex.Message });
        }

        return View(); 
    }
    [HttpGet]
    public async Task<IActionResult> Login(string? message)
    {
        if (message != null)
            ModelState.AddModelError("",message);
        return View(); 
    }

    [HttpPost]
    public async Task<IActionResult> Login(AuthViewModel model)
    {
        if (model.Email == null || model.Password == null)
            return RedirectToAction("Login", "Authenticate", new { message = "One or more fields are not filled!"});
        try
        {
            var user = await _db.User.Where(u=>u.Email == model.Email).SingleOrDefaultAsync();
            
            if (user == null) 
                return RedirectToAction("Login", "Authenticate", new { message = "User not found!"});
            
            if (user.Password != Hasher.HashPassword(model.Password))
                return RedirectToAction("Login", "Authenticate", new { message = "Wrong password!"});

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(await GetClaims(user)));

            return RedirectToAction("Index", "Home"); 

        }
        catch (Exception ex)
        {
            return RedirectToAction("Login", "Authenticate", new { message = ex.Message});
        }

    }
    
    private async Task<ClaimsIdentity> GetClaims(User user)
    {
        var accountRole = await _db.User.Include(r => r.Roles)
            .FirstOrDefaultAsync(u=>u.Id == user.Id);
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
        };

        foreach (var role in accountRole.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }
        
        return new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
}