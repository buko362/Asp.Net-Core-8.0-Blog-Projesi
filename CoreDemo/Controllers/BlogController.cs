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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
            
        BlogManager bm = new BlogManager(new EfBlogRepository()); //BlogManagerdan bir bm nesnesi türet bunu efblogrepository ile newle 
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        Context c = new Context();

        public BlogController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // [AllowAnonymous]
        public IActionResult Index()
        {
            //var values = bm.GetBlogListWithCategory();    
            var values = c.Blogs.Where(x => x.BlogStatus == true).Select(x => new BlogListItemViewModel
            {
                BlogID = x.BlogID,
                BlogTitle = x.BlogTitle,
                BlogImage = x.BlogImage,
                BlogCreateDate = x.BlogCreateDate,
                BlogContent = x.BlogContent,
                CategoryName = x.Category.CategoryName,
                CommentCount = x.Comments.Count(y => y.BlogID == x.BlogID),
                likeCount=x.LikeCount
            }).OrderByDescending(x=>x.BlogID).ToList();

            return View(values);
        }
        

        public async Task<IActionResult> BlogReadAll(int id)
        {
            ViewBag.i = id;
            ViewBag.CommentCount = c.Comments.Count(x => x.BlogID == id);
            ViewBag.writer = c.Blogs.Where(x => x.BlogID == id).Select(x => x.Writer.WriterName).FirstOrDefault();

            bool liked = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                liked = c.BlogLikes.Any(x => x.BlogID == id && x.AppUserId == user.Id);
            }
            ViewBag.Liked=liked;

            var br = c.BlogRatings.FirstOrDefault(x => x.BlogID == id);
            double avg = (br == null || br.BlogRatingCount == 0) ? 0 : (double)br.BlogTotalScore / br.BlogRatingCount;

            ViewBag.AvgRating=avg;
            ViewBag.RatingCount = br?.BlogRatingCount ?? 0;

            var values=bm.GetBlogByID(id);
            return View(values);
        }
        public IActionResult BlogListByWriter()
        {  
            var username = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault();
            var values= bm.GetListWithCategoryByWriterBm(writerID);
            return View(values);
        }
        [HttpGet]
        public IActionResult BlogAdd()
        {
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                   Text=x.CategoryName,
                                                   Value=x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.cv=categoryValues;//categoryvalues değişkeninden gelen değerleri dropdown ile taşıyacağız
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(BlogAddViewModel model)
        {
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.cv=categoryValues;
            if(!ModelState.IsValid)
                return View(model);

            var user=await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Index","Login");

            var writerID=c.Writers.Where(x=>x.WriterMail==user.Email).Select(x=>x.WriterID).FirstOrDefault();

            string? imagePath = null;
            if(model.BlogImageFile!=null && model.BlogImageFile.Length > 0)
            {
                var extention = Path.GetExtension(model.BlogImageFile.FileName);
                var newImageName= Guid.NewGuid() + extention;
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImageFiles/");
                Directory.CreateDirectory(folder);

                var location = Path.Combine(folder, newImageName);
                using var stream = new FileStream(location, FileMode.Create);
                await model.BlogImageFile.CopyToAsync(stream);

                imagePath = "/BlogImageFiles/" + newImageName;
            }

            var blog = new Blog
            {
                BlogTitle = model.BlogTitle,
                BlogContent = model.BlogContent,
                CategoryID = model.CategoryId,
                BlogImage = imagePath,
                BlogCreateDate = DateTime.Now,
                BlogStatus = true,
                WriterID = writerID
            };
            bm.TAdd(blog);
            return RedirectToAction("BlogListByWriter", "Blog");
        }
        public IActionResult DeleteBlog(int id)
        {
            var blogvalue=bm.TGetByID(id);
            bm.TDelete(blogvalue);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            var blog = bm.TGetByID(id);
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.cv = categoryValues;
            var model = new BlogUpdateViewModel
            {
                BlogId = blog.BlogID,
                BlogTitle = blog.BlogTitle,
                BlogContent = blog.BlogContent,
                CategoryId = blog.CategoryID,
                ExistingBlogImage = blog.BlogImage
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBlog(BlogUpdateViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.BlogTitle) || string.IsNullOrWhiteSpace(model.BlogContent))
            {
                ModelState.AddModelError("", $"Title/Content gelmiyor. Title='{model.BlogTitle}', ContentLength={(model.BlogContent?.Length ?? 0)}");
                return View(model);
            }

            List<SelectListItem> categoryValues=(from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text=x.CategoryName,
                                                     Value=x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.cv = categoryValues;

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Index", "Login");

            var writerID = c.Writers.Where(x => x.WriterMail == user.Email).Select(x => x.WriterID).FirstOrDefault();

            var blog = bm.TGetByID(model.BlogId);
            if (blog == null)
                return NotFound();

            if (blog.WriterID != writerID)
                return Forbid();

            blog.BlogTitle = model.BlogTitle;
            blog.BlogContent = model.BlogContent;
            blog.CategoryID = model.CategoryId;
            blog.BlogStatus = true;

            if(model.NewBlogImageFile!=null && model.NewBlogImageFile.Length > 0)
            {
                var extention = Path.GetExtension(model.NewBlogImageFile.FileName);
                var newImageName = Guid.NewGuid() + extention;

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImageFiles/");
                Directory.CreateDirectory(folder);

                var location = Path.Combine(folder, newImageName);
                using var stream = new FileStream(location, FileMode.Create);
                await model.NewBlogImageFile.CopyToAsync(stream);

                blog.BlogImage = "/BlogImageFiles/" + newImageName; 
            }


            bm.TUpdate(blog);
            return RedirectToAction("BlogListByWriter");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var user=await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var blog = await c.Blogs.FirstOrDefaultAsync(x => x.BlogID == id);
            if(blog==null) return NotFound();

            var existing = await c.BlogLikes.FirstOrDefaultAsync(x => x.BlogID == id && x.AppUserId == user.Id);
            if (existing == null)
            {
                c.BlogLikes.Add(new BlogLike
                {
                    BlogID = id,
                    AppUserId=user.Id
                });

                blog.LikeCount += 1;
                await c.SaveChangesAsync();
                return Ok(new { liked = true, likeCount = blog.LikeCount });
            }
            else
            {
                c.BlogLikes.Remove(existing);
                blog.LikeCount = Math.Max(0, blog.LikeCount - 1);
                await c.SaveChangesAsync();

                return Ok(new { liked = false, likeCount = blog.LikeCount });
            }
                
        }
    }
}
