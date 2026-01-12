using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Admin
{
    public class AdminMessageNotification:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        Context c = new Context();

        public AdminMessageNotification(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var writerID = c.Writers.Where(x => x.AppUserId == user.Id).Select(x => x.WriterID).FirstOrDefault();

            var message = c.Message2s.Include(x => x.SenderUser).Where(x => x.ReceiverID == writerID && x.MessageStatus == true).OrderByDescending(x => x.MessageID).Take(3).ToList();
            return View(message);
        }
    }
}
