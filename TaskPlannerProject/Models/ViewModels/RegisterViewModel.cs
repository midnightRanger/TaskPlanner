using System.ComponentModel.DataAnnotations;

namespace TaskPlannerProject.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Fill the login field!")]
    [Display(Name="Login")]
    public string? Login { get; set; }
    [Required(ErrorMessage = "Fill the login field!")]
    [Display(Name="Email")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Fill the password field!")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
 
    [Required(ErrorMessage = "Repeat the password")]
    [Compare("Password", ErrorMessage = "Passwords are different! ")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string PasswordConfirm { get; set; }
}