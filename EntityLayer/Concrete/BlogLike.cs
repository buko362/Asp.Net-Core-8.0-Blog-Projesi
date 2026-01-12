using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class BlogLike
    {
        public  int BlogLikeID { get; set; }
        public  int BlogID { get; set; }
        public Blog Blog { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
