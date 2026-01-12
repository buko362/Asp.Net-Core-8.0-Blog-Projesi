
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Moderator,Writer")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly Context c=new Context();

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateViewModel model = new UserUpdateViewModel();
            model.mail = values.Email;
            model.namesurname = values.NameSurname;
            model.imageurl = values.ImageUrl;
            model.username = values.UserName;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserUpdateViewModel model)
        {
            var values = await _userManager.GetUserAsync(User);
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
                var extension = Path.GetExtension(model.imagefile.FileName);
                var newImageName = Guid.NewGuid().ToString() + extension;
                var folder=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/AdminImageFiles/");
                if(!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var location = Path.Combine(folder, newImageName);

                using (var stream = new FileStream(location, FileMode.Create))
                {
                    await model.imagefile.CopyToAsync(stream);
                } 
                
                values.ImageUrl ="/AdminImageFiles/"+ newImageName;
            }


            if (model.ChangePassword && !string.IsNullOrWhiteSpace(model.password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(values);
                var passResult = await _userManager.ResetPasswordAsync(values, token, model.password);
                if (!passResult.Succeeded)
                {
                    foreach (var item in passResult.Errors)
                        ModelState.AddModelError("", item.Description);

                    model.imageurl = values.ImageUrl;
                    return View(model);
                }
            }
            var result=await _userManager.UpdateAsync(values);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);

                model.imageurl = values.ImageUrl;
                return View(model);
            }

            var writer = c.Writers.FirstOrDefault(x => x.AppUserId == values.Id);
            if (writer != null)
            {
                writer.WriterMail = values.Email;
                writer.WriterName = values.NameSurname;

                c.Writers.Update(writer);
                await c.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(values);
            return RedirectToAction("Index", "Profile");
        }
    }
}
