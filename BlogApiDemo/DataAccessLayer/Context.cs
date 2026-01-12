using Microsoft.EntityFrameworkCore;

namespace BlogApiDemo.DataAccessLayer
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //burada connection stringi tanımlayacağız
        {
            optionsBuilder.UseSqlServer("server=BUKET\\SQLEXPRESS01;database=CoreBlogAPIDb;integrated security=true; TrustServerCertificate=true;"); //tanımladık
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
