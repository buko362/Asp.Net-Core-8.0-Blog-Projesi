using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    public class RegisterController : Controller
    {
        WriterManager wm=new WriterManager(new EfWriterRepository());
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Writer p)
        {
            WriterValidator wv = new WriterValidator();
            ValidationResult results = wv.Validate(p); //parametreden gelen değerleri kontrol et
            if(results.IsValid)//eğer sonuçlar geçerliyse
            {
                p.WriterStatus = true;
                p.WriterAbout = "deneme test";
                wm.TAdd(p);//parametreden gelen değeri wm içine ekleyeceğiz
                return RedirectToAction("Index", "Blog"); //blog controllerın  içindeki index actionına git
            }
            else
            {
                foreach(var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);//hatayı veren propertyname, hata mesajı
                }
            }
           return View();
           
        }


    }
}
