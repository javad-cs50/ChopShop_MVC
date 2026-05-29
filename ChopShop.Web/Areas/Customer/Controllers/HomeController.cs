using ChopShop.DataAccess.Data;
using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;


namespace ChopShopWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }
    public IActionResult Index(string? searchTerm, int? categoryId)
    {
        var categories = _unitOfWork.Category.GetAll().OrderBy(c => c.DisplayOrder).ToList();
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        var products = new List<Product>();
        if (string.IsNullOrEmpty(searchTerm) && categoryId == null)
        {
             products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }
        else if (string.IsNullOrEmpty(searchTerm) && categoryId != null)
        {
            products = _unitOfWork.Product.GetAll(includeProperties: "Category").Where(p => p.CategoryId == categoryId).ToList();
            return View(products);
        }
        else if (!string.IsNullOrEmpty(searchTerm) && categoryId == null)
        {
            products = _unitOfWork.Product.Search(searchTerm);
            return View(products);
        }
        else
        {
            products = _unitOfWork.Product.Search(searchTerm).Where(p => p.CategoryId == categoryId).ToList();
            return View(products);
        }
    
    }
   
    
    public IActionResult Details(int id)
    {
        var product = _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Category");
        var cart = new ShoppingCart
        {
            Product = product,
            Count = 1,
            ProductId = id
        };
        return View(cart);
    }
    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.UserId = userId;
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.UserId == shoppingCart.UserId && sc.ProductId == shoppingCart.ProductId);
        if (cartFromDb == null)
        {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
        }
        else
        {
            cartFromDb.Count = cartFromDb.Count + shoppingCart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        _unitOfWork.Save();
        return RedirectToAction("Index", "Home", new { area = "Customer" });
    }
}
