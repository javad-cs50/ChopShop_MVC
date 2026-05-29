using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;
using ChopShop.Models.ViewModels;
using ChopShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChopShop.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index(string? orderStatus)
    {
        IEnumerable<OrderHeader> listOfOrders;
        if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
        {
            if (orderStatus != null)
            {
                listOfOrders = _unitOfWork.OrderHeader.GetAll(includeProperties: "User").Where(o => o.OrderStatus == orderStatus);
            }
            else
            {
                listOfOrders = _unitOfWork.OrderHeader.GetAll(includeProperties: "User");
            }
        }
        else
        {
            var userClaims = (ClaimsIdentity)User.Identity;
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (orderStatus != null)
            {
                listOfOrders = _unitOfWork.OrderHeader.GetAll(includeProperties: "User").Where(o => o.UserId == userId && o.OrderStatus == orderStatus);
            }
            else
            {
                listOfOrders = _unitOfWork.OrderHeader.GetAll(includeProperties: "User").Where(o => o.UserId == userId);
            }

        }

        return View(listOfOrders);
    }
    public IActionResult Details(int orderId)
    {
        var orderItems = _unitOfWork.OrderDetail.GetAll(includeProperties: "Product").Where(o => o.OrderHeaderId == orderId).ToList();
        var orderHeader = _unitOfWork.OrderHeader.Get(o => o.ID == orderId, includeProperties: "User");
        var orderVM = new OrderVM
        {
            OrderDetails = orderItems,
            OrderHeader = orderHeader
        };

        return View(orderVM);

    }
    [HttpPost]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public IActionResult StartProcessing(int orderId)
    {
        var orderToProcess = _unitOfWork.OrderHeader.Get(o => o.ID == orderId);
        orderToProcess.OrderStatus = SD.StatusInProcess;
        _unitOfWork.Save();
        TempData["Success"] = "Order In Processing successfully";

        return RedirectToAction("Details", new { orderId = orderToProcess.ID });

    }
    [HttpPost]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + SD.Role_Customer)]

    public IActionResult Cancel(int orderId)
    {
        var orderToCancel = _unitOfWork.OrderHeader.Get(o => o.ID == orderId);
        orderToCancel.OrderStatus = SD.StatusCancelled;
        _unitOfWork.Save();
        TempData["Success"] = "Order Cancelled successfully";
        return RedirectToAction("Details", new { orderId = orderToCancel.ID });
    }
    public IActionResult Logs()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public IActionResult Logs(DateTime startDate, DateTime endDate)
    {
        if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
        {
            return View();
        }
        var orderLogsVM = new OrderLogsVM();
        orderLogsVM.StartDate = startDate;
        orderLogsVM.EndDate = endDate;
        var listOfOrders = _unitOfWork.OrderHeader.GetAll().Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.OrderStatus != SD.StatusCancelled);
        orderLogsVM.ListOfOrders = listOfOrders;
        orderLogsVM.CountOfOrders = listOfOrders.Count();

        //fetch Order Details From DB And store it in a list
        var listOfOrderDetails = new List<OrderDetail>();
        foreach (var orderHeader in listOfOrders)
        {
            var orderDetails = _unitOfWork.OrderDetail.GetAll().Where(o => o.OrderHeaderId == orderHeader.ID);
            listOfOrderDetails.AddRange(orderDetails);
        }
        //Count of orders
        orderLogsVM.CountOfOrders = listOfOrders.Count();
        //Calculate Total Fee of Orders
        decimal totalFee = 0;
        foreach (var order in listOfOrders)
        {
            totalFee += order.OrderTotal;

        }
        orderLogsVM.TotalFee = (double)totalFee;

        var itemsCount = listOfOrderDetails.Sum(o => o.Count);
        orderLogsVM.CountOfSelledItems = itemsCount;


        //Most Value Order
        OrderHeader mostValueOrder = new();
        foreach (var order in listOfOrders)
        {
            if (mostValueOrder.OrderTotal < order.OrderTotal)
            {
                mostValueOrder = order;
            }
        }
        orderLogsVM.MostValueOrder = mostValueOrder;

        //Top Selled Product
        var topProduct = _unitOfWork.OrderDetail.GetAll(includeProperties: "Product")
            .Where(o => o.OrderHeader.OrderDate >= startDate && o.OrderHeader.OrderDate < endDate && o.OrderHeader.OrderStatus != SD.StatusCancelled)
            .GroupBy(o => o.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalCount = g.Sum(x => x.Count)
            })
            .OrderByDescending(x => x.TotalCount).Join(_unitOfWork.Product
            .GetAll(), x => x.ProductId, p => p.Id, (x, p) =>
            new TopSelledProductVM
            {
                ProductId = p.Id,
                Title = p.Title,
                ImgUrl = p.ImageUrl,
                TotalCount = x.TotalCount
            }).Take(3).ToList();
        orderLogsVM.TopSelledProducts = topProduct;

        return View("LogsSummary", orderLogsVM);
    }
    public IActionResult LogsSummary(OrderLogsVM logsVM)
    {
        return View(logsVM);
    }

}
