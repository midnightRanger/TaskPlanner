using Microsoft.AspNetCore.Authentication.Cookies;
using TaskPlannerProject.Helper;
using TaskPlannerProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TaskPlannerProject.Models.DbContext>(options =>
{
    options.UseNpgsql($"server={AliasDb.Server};User Id={AliasDb.UserId};Password={AliasDb.Password};" +
                      $"Port={AliasDb.Port};Database={AliasDb.Database};",
        o => o.UseNodaTime());  
    options.LogTo(Console.WriteLine);
    options.EnableSensitiveDataLogging();   
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Authenticate/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Authenticate/Login");
        
    });
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();