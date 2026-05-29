using System.ComponentModel.DataAnnotations;

namespace ChopShop.Models.ViewModels;

public class UpdateUserVM
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    [MinLength(8)]
    public string CurrentPassword { get; set; }
    [MinLength(8)]
    public string NewPassword { get; set; }
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
    public string Role { get; set; } = string.Empty;
}
