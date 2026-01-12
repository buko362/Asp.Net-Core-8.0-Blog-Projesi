using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Extensions;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]//areanın adını yazıyoruz tırnak içine-- böylece category controller içinde oluşturduğumuz actionların areadan geldiğini programa bildirmiş oluyoruz 
    [Authorize(Roles ="Admin,Moderator")]
    public class CategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        public IActionResult Index(int page=1)
        {
            var values = cm.GetList().ToPagedList(page,3);//başlangıç parametresi (sayfalama işlemi kaçıncı sayfadan başlayacak),her bir sayfada kaç tane değerin olacak
            return View(values);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category p)
        {
            
            CategoryValidator cv = new CategoryValidator();
            ValidationResult results = cv.Validate(p);
            
            if (results.IsValid)//eğer sonuçlar geçerliyse
            {
                p.CategoryStatus = true;
                cm.TAdd(p);
                return RedirectToAction("Index", "Category"); 
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);//hatayı veren propertyname, hata mesajı
                }
            }
            return View();
        }
        public IActionResult DeleteCategory(int id)
        {
            var value = cm.TGetByID(id);
            cm.TDelete(value);
            return RedirectToAction("Index");
        }
    }
}
