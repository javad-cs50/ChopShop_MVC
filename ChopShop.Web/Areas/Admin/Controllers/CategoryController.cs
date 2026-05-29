using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;
using ChopShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChopShopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin +","+SD.Role_Employee)]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var categories = _unitOfWork.Category.GetAll().OrderBy(c=>c.DisplayOrder).ToList();
        return View(categories);
    }
    public IActionResult Upsert(int? id)
    {
        //Create
        if (id == null || id < 1)
        {
            return View(new Category());
        }
        //update
        else
        {
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
    [HttpPost]
    public IActionResult Upsert(Category? category)
    {
        if (ModelState.IsValid)
        {
            //Create
            if (category.Id<=0)
            {
                //for preventing to add duplicated Category(Validate by name)
                var categoryFromDb = _unitOfWork.Category.Get(c => c.Name == category.Name);
                if (categoryFromDb != null)
                {
                    ModelState.AddModelError("", "The Category with the same name is exist!");

                    TempData["Error"] = "The Category with the same name is exist!" +
                        "Try Another Name";
                    
                    return View(category);
                }
                _unitOfWork.Category.Add(category);
                TempData["Success"] = "The Category Added Successfully";
            }
            //Update
            else
            {
                var isNameAndOrderSame = _unitOfWork.Category.Get(c => c.Name == category.Name && c.DisplayOrder==category.DisplayOrder);
                if (isNameAndOrderSame != null)
                {
                    ModelState.AddModelError("", "The Category with the same name and same Display Order is exist!");

                    TempData["Error"] = "The Category with the same name is exist!" +
                        "Try Another Name";
                    
                    return View(category);
                }
                _unitOfWork.Category.Update(category);
                TempData["Success"] = "The Category Updated Successfully";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index", "Category");
        }
        else
            return View(category);
    }

    //public IActionResult Delete(int? id)
    //{
    //    var category = _unitOfWork.Category.Get(c => c.Id == id);
    //    if (category == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(category);
    //}
    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePost(int? id)
    //{
    //    var category = _unitOfWork.Category.Get(c => c.Id == id);
    //    if (category == null)
    //    {
    //        return NotFound();
    //    }
    //    _unitOfWork.Category.Delete(category);
    //    _unitOfWork.Save();
    //    TempData["Success"] = "The Category Deleted Successfully";

    //    return RedirectToAction("Index", "Category");

    //}
    #region Api Calls
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return Json(new { success = false, message = "id is null" });
        }
        var categoryToDelete = _unitOfWork.Category.Get(c => c.Id == id);
        if(categoryToDelete==null)
        {
            return Json(new { success = false, message = "invalid id" });

        }
        else
        {
            _unitOfWork.Category.Delete(categoryToDelete);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product Delete Successfully" });
        }

    }
    #endregion
}