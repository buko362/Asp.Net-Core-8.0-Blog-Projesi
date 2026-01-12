using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        Context c = new Context();//çok sağlıklı bir yöntem değil

        public WriterController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            //var userMail = User.Identity.Name;//aktif kullanıcının name değerini alır
            //ViewBag.v = userMail;//usermailden gelen değeri taşı
            //var writerName = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterName).FirstOrDefault();
            //ViewBag.v2 = writerName;
            return View();
        }
        public IActionResult WriterProfile()
        {
            return View();
        }

        public IActionResult WriterMail()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Test()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult WriterFooterPartial()
        {
            return PartialView();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> WriterEditProfile()
        {
            var values = await _userManager.GetUserAsync(User);
            if (values == null)
                return RedirectToAction("Index", "Login");
            var writer = c.Writers.FirstOrDefault(x => x.AppUserId == values.Id);
            UserUpdateViewModel model = new UserUpdateViewModel();
            model.mail = values.Email;
            model.namesurname = values.NameSurname;
            model.imageurl = values.ImageUrl;
            model.username = values.UserName;
            model.about = writer?.WriterAbout;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> WriterEditProfile(UserUpdateViewModel model)
        {

            var values = await _userManager.GetUserAsync(User);
            if (values == null)
                return RedirectToAction("Index","Login");

            var writer = c.Writers.FirstOrDefault(x => x.AppUserId == values.Id);

            if (model.ChangePassword && string.IsNullOrWhiteSpace(model.password))
            {
                ModelState.AddModelError("password", "Şifre zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                model.imageurl = values.ImageUrl;
                return View(model);
            }

            values.NameSurname = model.namesurname;
            values.Email = model.mail;
            values.UserName = model.username;

            if (model.imagefile != null && model.imagefile.Length>0)
            {
                var extention = Path.GetExtension(model.imagefile.FileName);
                var newImageName = Guid.NewGuid().ToString() + extention;
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var location = Path.Combine(folder, newImageName);
                using (var stream = new FileStream(location, FileMode.Create))
                {
                    await model.imagefile.CopyToAsync(stream);
                }
                values.ImageUrl = "/WriterImageFiles/" + newImageName;
            }

            

            if (model.ChangePassword && !string.IsNullOrWhiteSpace(model.password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(values);
                var passResult=await _userManager.ResetPasswordAsync(values,token,model.password);
                if (!passResult.Succeeded)
                {
                    foreach (var item in passResult.Errors)
                        ModelState.AddModelError("", item.Description);

                    model.imageurl = values.ImageUrl;
                    return View(model);
                }
            }
            
            var result = await _userManager.UpdateAsync(values);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);

                model.imageurl = values.ImageUrl;
                return View(model);
            }

            if (writer != null)
            {
                writer.WriterAbout = model.about;
                writer.WriterMail = values.Email;
                writer.WriterName = values.NameSurname;

                c.Writers.Update(writer);
                await c.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(values);

            return RedirectToAction("WriterEditProfile", "Writer");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult WriterAdd()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult WriterAdd(AddProfileImage p)
        {
            Writer w = new Writer();//writer sınıfından w nesnesi türettik
            if (p.WriterImage != null)
            {
                var extension = Path.GetExtension(p.WriterImage.FileName);
                var newimagename = Guid.NewGuid() + extension; //guid benzersiz isim tanımlamak için kullanılıyor
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                p.WriterImage.CopyTo(stream);
                w.WriterImage = newimagename;
            }
            w.WriterMail = p.WriterMail;
            w.WriterName = p.WriterName;
            w.WriterPassword = p.WriterPassword;
            w.WriterStatus = true;
            w.WriterAbout = p.WriterAbout;

            wm.TAdd(w);
            return RedirectToAction("Index", "Dashboard");
        }
    }

}
