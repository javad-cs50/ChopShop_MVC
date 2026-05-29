using System.ComponentModel.DataAnnotations;

namespace ChopShop.Models.ViewModels;

public class CreateUserVM
{
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    [MinLength(8)]
    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string Role { get; set; } = string.Empty;
}
