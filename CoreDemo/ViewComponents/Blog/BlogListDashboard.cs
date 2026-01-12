using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Blog
{
    public class BlogListDashboard:ViewComponent
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        public IViewComponentResult Invoke()
        {                                            //blogIDyi büyükten küçüğe sıralar
            var values = bm.GetBlogListWithCategory().OrderByDescending(x=>x.BlogID).Take(10).ToList();//son 10 blogu getir
            return View(values);
        }
        
    }
}
