using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Comment
{
    public class AddComment:ViewComponent
    {
        public IViewComponentResult Invoke(int id)
        {
            ViewBag.BlogId=id;
            return View();
        }
    }
}
