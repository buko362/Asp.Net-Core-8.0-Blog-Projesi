
using CoreDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDemo.Controllers
{
    public class ViewModelController : Controller
    {
        public IActionResult Index()
        {
            var model = new ViewModel();
            model.Sehirler.Add(new SelectListItem() { Text = "İstanbul", Value = "1", Selected = false });
            model.Sehirler.Add(new SelectListItem() { Text = "Ankara", Value = "2", Selected = false });
            model.Sehirler.Add(new SelectListItem() { Text = "İzmir", Value = "3", Selected = false });
            model.Sehirler.Add(new SelectListItem() { Text = "Diğer", Value = "4", Selected = false });
            return View(model);
        }
    }

    
}
