namespace CoreDemo.Models
{
    public class BlogListItemViewModel
    {
        public int BlogID { get; set; }
        public string? BlogTitle { get; set; }
        public string? BlogImage { get; set; }
        public DateTime? BlogCreateDate { get; set; }
        public string? BlogContent { get; set; }
        public string? CategoryName { get; set; }
        public int? CommentCount { get; set; }
        public int? likeCount { get; set; }
    }
}
