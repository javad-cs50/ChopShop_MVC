using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChopShop.Models.ViewModels;

public class UserWithRoleVM
{
    public IdentityUser User { get; set; }
    public string Role { get; set; }
}
