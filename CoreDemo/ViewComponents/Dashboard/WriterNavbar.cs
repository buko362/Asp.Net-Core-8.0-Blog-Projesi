using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Dashboard
{
    public class WriterNavbar:ViewComponent
    {
        Context c=new Context();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            var writerID=c.Writers.Where(x=>x.WriterMail==usermail).Select(x=>x.WriterID).FirstOrDefault();
            ViewBag.name=c.Users.Where(x=>x.Email==usermail).Select(x=>x.NameSurname).FirstOrDefault();
            ViewBag.userMail=usermail;
            ViewBag.image = c.Users.Where(x => x.Email == usermail).Select(x => x.ImageUrl).FirstOrDefault();
            return View();
        }

    }
}
