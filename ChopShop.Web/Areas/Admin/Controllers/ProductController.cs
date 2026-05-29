using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;
using ChopShop.Models.ViewModels;
using ChopShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace ChopShopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{SD.Role_Admin} " + "," + $"{SD.Role_Employee}")]


public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<ProductController> _logger;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ILogger<ProductController> logger)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }
    public IActionResult Index()
    {
        var product = _unitOfWork.Product.GetAll(includeProperties: "Category").OrderBy(p => p.Category.DisplayOrder).ToList();

        return View(product);
    }
    public IActionResult Upsert(int? id)

    {
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });
        if (id == null || id < 1)
        {
            var productVM = new ProductVM
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            return View(productVM);
        }
        else
        {
            var product = _unitOfWork.Product.Get(p => p.Id == id);
            var productVM = new ProductVM
            {
                CategoryList = CategoryList,
                Product = product
            };
            return View(productVM);
        }
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM productVM, IFormFile? file)
    {
        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath,"Images","Product");
                
                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))

                {
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\','/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                
                Directory.CreateDirectory(productPath);
                Console.Clear();
                Console.WriteLine(fileName);
                Console.WriteLine(productPath);
                Console.WriteLine(wwwRootPath);
                _logger.LogDebug(fileName);
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                //productVM.Product.ImageUrl = @"Images\Product\" + fileName;
                productVM.Product.ImageUrl = $"/Images/Product/{fileName}";
            }
            if (productVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
                TempData["Success"] = "Product Successfully Added.";
            }
            else if (productVM.Product.Id > 0)
            {
                _unitOfWork.Product.Update(productVM.Product);
                TempData["Success"] = "Product Successfully Updated.";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index", "Product");
        }
        return View(productVM);
    }

    //public IActionResult Delete(int? id)
    //{
    //    if (id == null || id == 0) { return NotFound(); }
    //    var product = _unitOfWork.Product.Get(p => p.Id == id);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(product);
    //}
    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePost(int id)
    //{
    //    var product = _unitOfWork.Product.Get(p => p.Id == id);
    //    if (product == null) { return NotFound(); }
    //    _unitOfWork.Product.Delete(product);
    //    _unitOfWork.Save();
    //    var wwwRootPath = _webHostEnvironment.WebRootPath;
    //    var imgPath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

    //    if (System.IO.File.Exists(imgPath))
    //    {
    //        System.IO.File.Delete(imgPath);
    //    }

    //    TempData["Success"] = "Product Deleted Successfully";
    //    return RedirectToAction("Index", "Product");

    //}
    #region Api Calls
    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = products });
    }
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return Json(new { success = false, message = "Invalid Id" });
        }
        var product = _unitOfWork.Product.Get(p => p.Id == id);
        if (product == null)
        {
            return Json(new { success = false, message = "Error While Deleting!" });
        }
        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        _unitOfWork.Product.Delete(product);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Product Has Been Deleted" });
    }
    #endregion
}
