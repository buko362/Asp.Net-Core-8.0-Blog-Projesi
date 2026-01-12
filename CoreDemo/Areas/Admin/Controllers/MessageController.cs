using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator,Writer")]
    public class MessageController : Controller
    {
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        Context c = new Context();
        public IActionResult InBox()
        {
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(x => x.WriterID).FirstOrDefault();
            var values = mm.GetInboxListByWriter(writerID);
            return View(values);
        }
        public IActionResult SendBox()
        {
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(x => x.WriterID).FirstOrDefault();
            var values = mm.GetSendBoxListByWriter(writerID);
            return View(values);
        }
        [HttpGet]
        public IActionResult ComposeMessage(int? replyToID)
        {
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(x => x.WriterID).FirstOrDefault();
            var receiverUsers = c.Writers.Where(x => x.WriterID != writerID).Select(x => new SelectListItem
            {
                Text = x.WriterMail,
                Value = x.WriterID.ToString()
            }).ToList();

            if (replyToID.HasValue)
            {
                var msg = c.Message2s.Include(x => x.SenderUser).FirstOrDefault(x => x.MessageID == replyToID.Value);
                if (msg != null)
                {
                    ViewBag.SelectedReceiverId = msg.SenderID;

                    ViewBag.DefaultSubject = !string.IsNullOrEmpty(msg.Subject) && msg.Subject.StartsWith("Re:") ? msg.Subject : "Re: " + msg.Subject;

                    ViewBag.DefaultDetails = $@"
-----Orijinal Mesaj------
Tarih: {msg.MessageDate}
Kimden: {msg.SenderUser?.WriterMail}
Konu: {msg.Subject}

{msg.MessageDetails}";
                }
            }

            ViewBag.receiver = new SelectList(receiverUsers,"Value","Text",ViewBag.SelectedReceiverId);
            return View();
        }
        [HttpPost]
        public IActionResult ComposeMessage(Message2 p)
        {
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(x => x.WriterID).FirstOrDefault();
            p.SenderID = writerID;
            p.MessageDate = DateTime.Now;
            p.MessageStatus = true;
            mm.TAdd(p);
            return RedirectToAction("SendBox");
        }

        public IActionResult MailDetails(int id)
        {
            var value = c.Message2s.Include(x => x.SenderUser).Include(x => x.ReceiverUser).FirstOrDefault(x => x.MessageID == id);
            return View(value);
        }
    }
}
