using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }
        public string? BlogTitle { get; set; }
        public string? BlogContent { get; set; }
        public string? BlogThumbnailImage { get; set; }
        public string? BlogImage { get; set; }
        public DateTime? BlogCreateDate { get; set; }
        public bool? BlogStatus { get; set; }
        public int CategoryID { get; set; } //ilişki için categoryIDyi yazdık
        public Category Category { get; set; } //ilişki içinde alınacak tablo türünde ya da entity türünde tanımlanması gerekiyor
        public int WriterID { get; set; }
        public  int LikeCount { get; set; }
        public Writer Writer { get; set; }
        public List<Comment> Comments { get; set; }
        
    }
}
