using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class CommentController : Controller
    {
        CommentManager cm = new CommentManager(new EfCommentRepository());
        Context c = new Context();

        public IActionResult Index()
        {
            var values = cm.GetCommentWithBlog();
            return View(values);
        }

        [HttpPost]
        public IActionResult DeleteComment(int id)
        {
            var comment = c.Comments.FirstOrDefault(x => x.CommentID == id);
            if (comment == null)
                return RedirectToAction("Index");

            int? blogIdNullable = comment.BlogID;
            int score = comment.BlogScore;

            c.Comments.Remove(comment);

            if(blogIdNullable != null && score > 0)
            {
                int blogId = blogIdNullable.Value;

                var br = c.BlogRatings.FirstOrDefault(x => x.BlogID == blogId);
                if(br!=null && br.BlogRatingCount > 0)
                {
                    br.BlogTotalScore -= score;
                    br.BlogRatingCount -= 1;

                    if(br.BlogRatingCount <= 0)
                    {
                        br.BlogRatingCount = 0;
                        br.BlogTotalScore = 0;
                    }
                    if (br.BlogTotalScore < 0)
                        br.BlogTotalScore = 0;
                }
            }
            c.SaveChanges();
            TempData["CommentMsg"] = "Yorum silindi ve blog puanı güncellendi.";
            return RedirectToAction("Index");
        }
    }
}
