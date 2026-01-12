using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Blog
{
    public class WriterLastBlog : ViewComponent
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        public IViewComponentResult Invoke(int id)
        {
            var blog = bm.TGetByID(id);
            var values = bm.GetBlogListWithWriter(blog.WriterID).Where(x=>x.BlogID!=id).OrderByDescending(x=>x.BlogID).Take(3).ToList();
            return View(values);
        }
    }
}
