using Microsoft.AspNetCore.Http;
namespace CoreDemo.Models
{
    public class BlogAddViewModel
    {
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public int CategoryId { get; set; } 
        public IFormFile BlogImageFile { get; set; }
     
    }
}
