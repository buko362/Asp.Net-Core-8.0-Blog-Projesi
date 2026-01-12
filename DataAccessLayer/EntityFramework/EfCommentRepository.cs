using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfCommentRepository : GenericRepository<Comment>, ICommentDal
    {
        public List<Comment> GetListWithBlog()
        {
            using (var c=new Context())
            {
                return c.Comments.Include(x=>x.Blog).OrderByDescending(x=>x.CommentID).ToList();
            } 
        }

        public List<Comment> GetListWithBlogByID(int id)
        {
            using (var c=new Context())
            {
                return c.Comments.Where(x => x.BlogID == id && x.CommentStatus == true).Include(x => x.Blog).OrderByDescending(x => x.CommentID).ToList();
            }
        }
    }
}
