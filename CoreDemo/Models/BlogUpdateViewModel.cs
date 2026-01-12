using Microsoft.AspNetCore.Http;
namespace CoreDemo.Models
{
    public class BlogUpdateViewModel
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public int CategoryId { get; set; }
        public string? ExistingBlogImage { get; set; }
        public IFormFile? NewBlogImageFile { get; set; }
    }
}
