using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;
using ChopShop.Models.ViewModels;
using ChopShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace ChopShop.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty]
    public ShoppingCartVM ShoppingCartVM { get; set; }
    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var  shoppingCartList=_unitOfWork.ShoppingCart.GetAll(includeProperties:"Product").Where(item=>item.UserId==userId).ToList();
        ShoppingCartVM = new()
        {
            ShoppingCartList = shoppingCartList,
            OrderHeader = new() { OrderTotal=0}
        };
        foreach(var item in ShoppingCartVM.ShoppingCartList)
        {
            ShoppingCartVM.OrderHeader.OrderTotal += item.Product.Price * item.Count;
        }

        return View(ShoppingCartVM);
    }
    public IActionResult Summary()
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var shoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Product").Where(item => item.UserId == userId).ToList();
        ShoppingCartVM = new()
        {
            ShoppingCartList = shoppingCartList,
            OrderHeader = new() { OrderTotal = 0,UserId=userId }
        };
        foreach (var item in ShoppingCartVM.ShoppingCartList)
        {
            ShoppingCartVM.OrderHeader.OrderTotal += item.Product.Price * item.Count;
        }

        return View(ShoppingCartVM);
    }
    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPost()
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var shoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Product").Where(item => item.UserId == userId).ToList();
        ShoppingCartVM.ShoppingCartList = shoppingCartList;
        ShoppingCartVM.OrderHeader.OrderDate=System.DateTime.UtcNow;
        ShoppingCartVM.OrderHeader.UserId = userId;
        foreach (var item in ShoppingCartVM.ShoppingCartList)
        {
            ShoppingCartVM.OrderHeader.OrderTotal += item.Product.Price * item.Count;
        }
        if (!ModelState.IsValid) return View(ShoppingCartVM);
        ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
        ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;

        _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        _unitOfWork.Save();
        foreach (var item in ShoppingCartVM.ShoppingCartList)
        {
            var orderDetail = new OrderDetail
            {
                ProductId = item.ProductId,
                OrderHeaderId = ShoppingCartVM.OrderHeader.ID,
                Price=(double)item.Product.Price,
                Count = item.Count
            };
            _unitOfWork.OrderDetail.Add(orderDetail);
        }
        _unitOfWork.Save();
        var clearShoppingCart = _unitOfWork.ShoppingCart.GetAll().Where(sc => sc.UserId == userId).ToList();
        _unitOfWork.ShoppingCart.DeleteRange(clearShoppingCart);
        _unitOfWork.Save();
        return RedirectToAction("OrderConfirmation", new {id=ShoppingCartVM.OrderHeader.ID});
    }
    public IActionResult OrderConfirmation(int id)
    {
        return View(id);
    }
    public IActionResult Plus(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc=>sc.Id==cartId);
        cartFromDb.Count += 1;
        _unitOfWork.ShoppingCart.Update(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction("Index");

    }
    public IActionResult Minus(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.Id == cartId);
        cartFromDb.Count -= 1;
        _unitOfWork.ShoppingCart.Update(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction("Index");

    }
    public IActionResult Remove(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.Id == cartId);
        _unitOfWork.ShoppingCart.Delete(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

}
