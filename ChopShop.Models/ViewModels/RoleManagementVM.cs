using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChopShop.Models.ViewModels;

public class RoleManagementVM
{
    
    public IdentityUser User { get; set; }
    public IEnumerable<SelectListItem> RoleList { get; set; }
    public string CurrentRole { get; set; }
}
