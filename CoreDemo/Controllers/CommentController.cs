using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class CommentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        CommentManager cm=new CommentManager(new EfCommentRepository());
        Context c=new Context();

        public CommentController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CommentAdd(Comment p)
        {
            if (p.BlogID == null)
                return BadRequest("BlogID boş olamaz");

            if(p.BlogScore <1 || p.BlogScore > 5)
            {
                TempData["RatingError"] = "Lütfen 1 ile 5 arasında yıldız seçin.";
                return RedirectToAction("BlogReadAll", "Blog", new { id = p.BlogID });
            }

            var user = await _userManager.GetUserAsync(User);

            p.CommentUserName = user.NameSurname;
            p.CommentDate= DateTime.Now;
            p.CommentStatus = true;
            p.CommentTitle = null;
            c.Comments.Add(p);
            await c.SaveChangesAsync();

            int blogId=(int)p.BlogID;
            var br = c.BlogRatings.FirstOrDefault(x => x.BlogID == blogId);

            if (br==null)
            {
                br = new BlogRating()
                {
                    BlogID = blogId,
                    BlogTotalScore = p.BlogScore,
                    BlogRatingCount = 1
                };
                c.BlogRatings.Add(br);
            }
            else
            {
                br.BlogTotalScore += p.BlogScore;
                br.BlogRatingCount += 1;
            }

            await c.SaveChangesAsync();
            return RedirectToAction("BlogReadAll", "Blog", new { id = blogId });
        }

    }
}
