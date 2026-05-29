using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChopShop.Models.ViewModels;

public class RegisterVM
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [MinLength(8)]
    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string PhoneNumber { get; set; }
}