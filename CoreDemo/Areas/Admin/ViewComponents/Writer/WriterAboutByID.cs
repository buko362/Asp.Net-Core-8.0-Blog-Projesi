using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreDemo.Areas.Admin.ViewComponents.Writer
{
    public class WriterAboutByID:ViewComponent
    {
        Context c = new Context();
        public IViewComponentResult Invoke(int id)
        {
            var writer = c.Writers.Include(x=>x.AppUser).FirstOrDefault(x => x.WriterID == id);
            return View(writer);
        }
    }
}
