using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EntityLayer.Concrete;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.User

{
    public class UserNavbar:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserNavbar(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            if(!User.Identity.IsAuthenticated)
                return View(); //login değil

            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            return View(user);
        }
    }
}
