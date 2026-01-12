using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Dashboard
{
    public class WriterSidebar:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public WriterSidebar(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        //Context c=new Context();

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.name = user.NameSurname;
            ViewBag.username = user.UserName;
            ViewBag.usermail= user.Email;
            ViewBag.image = user.ImageUrl;
            return View();

            //var username = User.Identity.Name;
            //var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            //var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(x => x.WriterID).FirstOrDefault();
            //ViewBag.name = c.Users.Where(x => x.Email == usermail).Select(x => x.NameSurname).FirstOrDefault();
            //ViewBag.userName = username;
            //ViewBag.userMail=usermail;
            //ViewBag.image = c.Users.Where(x => x.Email == usermail).Select(x => x.ImageUrl).FirstOrDefault();
            //return View();
        }
    }
}
