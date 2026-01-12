using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Category
{
    public class CategoryList:ViewComponent
    {
        CategoryManager cm= new CategoryManager(new EfCategoryRepository());
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var values = cm.GetList();
            ViewBag.count = c.Blogs.Where(x => x.BlogStatus == true).GroupBy(x => x.CategoryID).ToDictionary(g => g.Key, g => g.Count()); 
            return View(values);
        }
    }
}
