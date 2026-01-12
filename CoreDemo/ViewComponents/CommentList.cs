using CoreDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents
{
    public class CommentList:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var commentvalues = new List<UserComment>
            {
                new UserComment
                {
                    ID = 1,
                    Username="Buket"
                },
                new UserComment {
                    ID = 2,
                    Username="Mert Ege"
                },
                new UserComment {
                    ID=3,
                    Username="Burcu"
                }
            };
            return View(commentvalues);
        }
    }
}
