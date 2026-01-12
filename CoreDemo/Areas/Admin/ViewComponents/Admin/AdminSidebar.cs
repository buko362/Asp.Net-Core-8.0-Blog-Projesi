using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.ViewComponents.Admin
{
    public class AdminSidebar:ViewComponent
    {
        Context c=new Context();
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(x => x.WriterID).FirstOrDefault();
            ViewBag.name=c.Users.Where(x=>x.Email==usermail).Select(x=>x.NameSurname).FirstOrDefault();
            ViewBag.userMail=usermail;
            ViewBag.image = c.Users.Where(x => x.Email == usermail).Select(x => x.ImageUrl).FirstOrDefault();
            ViewBag.inboxCount = c.Message2s.Count(x => x.ReceiverID == writerID);
            ViewBag.sendboxCount = c.Message2s.Count(x => x.SenderID==writerID);
            return View();
        }
    }
}
