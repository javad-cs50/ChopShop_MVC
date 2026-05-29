using ChopShop.DataAccess.Data;
using ChopShop.Models.ViewModels;
using ChopShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChopShopWeb.Areas.Identity.Controllers;

[Area("Identity")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signinManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _signinManager = signinManager;
        _roleManager = roleManager;
        _db = db;
    }

    [HttpGet]
    [Authorize(Roles = $"{SD.Role_Admin} " + "," + $"{SD.Role_Employee}")]

    public async Task<IActionResult> Index(string? userRole, bool? isLock)
    {
        
        var users = _userManager.Users.ToList();
        if (isLock.HasValue && isLock==true)
        {
            users=users.Where(u=>u.LockoutEnd>DateTime.UtcNow).ToList();
        }
        var usersWithRole = new List<UserWithRoleVM>();

        var userRoles = _db.UserRoles.ToList();
        var roles = _db.Roles.ToList();
        //Adding Roles With Users
        foreach (var user in users)
        {
            var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            var role = _db.Roles.FirstOrDefault(r => r.Id == roleId).Name;
            var userVM = new UserWithRoleVM
            {
                User = user,
                Role = role
            };
            var userTypes = _db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });
            ViewBag.UserTypes = userTypes;
            usersWithRole.Add(userVM);
        }
        if (userRole !=null)
        {
            usersWithRole = usersWithRole.Where(u => u.Role == userRole).ToList();
            return View(usersWithRole);
        }
        return View(usersWithRole.OrderBy(u => u.Role).ToList());
    }
    [HttpGet]
    [Authorize(Roles = $"{SD.Role_Admin} " + "," + $"{SD.Role_Employee}")]

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [Authorize(Roles = $"{SD.Role_Admin} " + "," + $"{SD.Role_Employee}")]

    public async Task<IActionResult> Create(CreateUserVM userVM)
    {
        if (!ModelState.IsValid)
        {
            return View(userVM);
        }
        var createUser = new IdentityUser
        {
            Email = userVM.Email,
            UserName = userVM.Email,
            PhoneNumber = userVM.PhoneNumber,

        };
        var result = await _userManager.CreateAsync(createUser, userVM.Password);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("","User Didn't Add .");
            return View(userVM);
        }
        else
        {
            await _userManager.AddToRoleAsync(createUser, SD.Role_Customer);

            TempData["Success"] = "The User Successfully Added!";
            return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Customer" });
        }
    }
    public async Task<IActionResult> Update(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var userVM = new UpdateUserVM
        {
            Id = userId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber

        };
        return View(userVM);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateUserVM userVM)
    {
        if (!ModelState.IsValid)
        {
            return View(userVM);
        }
        var user = await _userManager.FindByEmailAsync(userVM.Email);

        if (user == null)
        {
            return RedirectToAction("Index");
        }
        user.Email = userVM.Email;
        user.PhoneNumber = userVM.PhoneNumber;
        user.UserName = userVM.Email;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            ModelState.AddModelError("","User Didn't Update .");
            return View(userVM);
        }
        var passwordResult = await _userManager.ChangePasswordAsync(user, userVM.CurrentPassword, userVM.NewPassword);
        if (!passwordResult.Succeeded)
        {
            ModelState.AddModelError("", "The Current Password isn't true .");

            return View(userVM);
        }
        if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
        {
            TempData["Success"] = "The User Successfully Updated!";
            return RedirectToAction("Index");

        }
        else
        {
            TempData["Success"] = "The User Successfully Updated!";
            return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Customer" });

        }

    }
    public IActionResult RoleManagement(string userId)
    {
        var user = _db.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound();
        }
        var roleList = _db.Roles.Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Name
        });
        var userRoleId = _db.UserRoles.FirstOrDefault(r => r.UserId == userId).RoleId;
        var userRoleName = _db.Roles.FirstOrDefault(r => r.Id == userRoleId).Name;
        var roleManagementVM = new RoleManagementVM
        {

            User = user,
            CurrentRole = userRoleName,
            RoleList = roleList
        };
        return View(roleManagementVM);
    }
    [HttpPost]
    public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
    {
        var roleId = _db.UserRoles.FirstOrDefault(r => r.UserId == roleManagementVM.User.Id).RoleId;
        var oldRole = _db.Roles.FirstOrDefault(r => r.Id == roleId).Name;
        if (roleManagementVM.CurrentRole != oldRole)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == roleManagementVM.User.Id);
            _userManager.RemoveFromRoleAsync(user, oldRole).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, roleManagementVM.CurrentRole).GetAwaiter().GetResult();

        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> LockUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();
        var lockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        user.LockoutEnabled = true;
        await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> UnlockUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();
        var lockoutEnd = DateTimeOffset.UtcNow;
        await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("index", "Home");
        }
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }

        var result = await _signinManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == loginVM.UserName);
            if (user.LockoutEnd > DateTime.UtcNow)
            {
                return RedirectToAction("UserLocked");
            }
            return RedirectToAction("index", "Home", new { area = "Customer" });
        }
        else if (result.IsLockedOut)
        {
            return RedirectToAction("UserLocked");
        }
        ModelState.AddModelError("", "Username or Password is Incorrect .");

        TempData["Warning"] = "Username or Password is Incorrect";
        return View(loginVM);

    }
    public IActionResult UserLocked()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { area = "Customer" });
    }
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }
        var user = await _userManager.FindByEmailAsync(registerVM.Email);

        if (user == null)
        {
            var createUser = new IdentityUser
            {
                Email = registerVM.Email,
                UserName = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber

            };
            var result = await _userManager.CreateAsync(createUser, registerVM.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(createUser, SD.Role_Customer);
                await _signinManager.SignInAsync(createUser, isPersistent: false);
                TempData["Success"] = "The User Successfully Added!";
                return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Customer" });
            }
            return RedirectToAction(actionName: "Index", controllerName: "Account", new { area = "Identity" });
        }
        else
        {
            ModelState.AddModelError("", "The User Already Exist.");

            TempData["Warning"] = "The User Already Exist!";
            return View(registerVM);
        }
    }


}



