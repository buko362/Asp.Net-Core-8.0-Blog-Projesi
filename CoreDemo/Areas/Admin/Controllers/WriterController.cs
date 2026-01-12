using BusinessLayer.Concrete;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WriterProfileList()
        {

            var values = wm.TGetListWithAppUser();
            return View(values);
        }

        public IActionResult WriterList()
        {
            var jsonWriters = JsonConvert.SerializeObject(writers);//writers nesnesindne gelen değeri jsona dönüştürüyoruz
            return Json(jsonWriters);
        }

        public IActionResult GetWriterByID(int writerid)
        {
            var findWriter=writers.FirstOrDefault(x=>x.ID==writerid);
            var jsonWriters = JsonConvert.SerializeObject(findWriter);
            return Json(jsonWriters);   
        }

        [HttpPost]
        public IActionResult AddWriter(WriterClass w)
        {
            writers.Add(w);
            var jsonWriters = JsonConvert.SerializeObject(w);
            return Json (jsonWriters);
        }

        public IActionResult DeleteWriter(int id)
        {
            var writer=writers.FirstOrDefault(x=>x.ID==id);
            writers.Remove(writer);
            return Json(writer);
        }

        public IActionResult UpdateWriter(WriterClass w)
        {
            var writer = writers.FirstOrDefault(x=>x.ID==w.ID);
            writer.Name=w.Name;
            var jsonWriter=JsonConvert.SerializeObject(w);
            return Json(jsonWriter);
        }

        public static List<WriterClass> writers = new List<WriterClass>()
        {
            new WriterClass
            {
                ID=1,
                Name="Mert"
            },
            new WriterClass
            {
                ID=2,
                Name="Ege"
            },
            new WriterClass
            {
                ID=3,
                Name="Buket"
            }

        };
    }
}
