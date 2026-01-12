using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DocumentFormat.OpenXml.Bibliography;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        Context c = new Context();

        public MessageController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult InBox() 
        { 
            var username = User.Identity.Name; 
            var userMail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault(); 
            var writerID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault(); 
            var values = mm.GetInboxListByWriter(writerID);
            return View(values); 
        }

        public IActionResult SendBox()
        {
            var username = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault();
            var values = mm.GetSendBoxListByWriter(writerID);
            return View(values);
        }

        public IActionResult MessageDetails(int id)
        {
            var value = mm.TGetByID(id);
            return View(value);
        }
        [HttpGet]
        public async Task<IActionResult> SendMessage()
        {
            var username = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var senderwriterID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault();
            var receiverUsers=c.Writers.Where(x=>x.WriterID!=senderwriterID).Select(x=>new SelectListItem
            {
                Text=x.WriterMail,
                Value=x.WriterID.ToString()
            }).ToList();

            ViewBag.receiver = receiverUsers;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message2 p)
        {
            var username = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault();
            p.SenderID = writerID;

            p.MessageStatus = true;
            p.MessageDate = DateTime.Now;
            mm.TAdd(p);
            return RedirectToAction("InBox");
        }
    }
}
