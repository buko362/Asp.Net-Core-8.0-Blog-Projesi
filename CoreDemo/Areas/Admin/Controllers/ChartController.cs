using CoreDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChartController : Controller
    {
        public IActionResult Index() //kategorilerin grafik üzerinde listelendiği action
        {
            return View();
        }

        public IActionResult CategoryChart() //metod
        {
            List<CategoryClass> list=new List<CategoryClass>();
            list.Add(new CategoryClass 
            { 
                categoryname = "Teknoloji",
                categorycount=10 
            });
            list.Add(new CategoryClass
            {
                categoryname="Yazılım",
                categorycount=8
            });
            list.Add(new CategoryClass
            {
                categoryname = "Sanat",
                categorycount = 4
            });
            list.Add(new CategoryClass
            {
                categoryname = "Film & Dizi",
                categorycount = 12
            });
            return Json(new {jsonList=list }); //chartları json formatında çağırıyoruz, listten gelen değeri atasın
        }
    }
}
