using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>  //DbContext vardı Identity kütüphanesinde değiştirdik, IdentityDbContext DbContextten miras alıyor.
    {                                          //parametre türü         parametre                                                
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //burada connection stringi tanımlayacağız
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config=new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json",optional:true)
                    .AddJsonFile("appsettings.Development.json",optional:true)
                    .AddEnvironmentVariables()
                    .Build();

                var cs = config.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrWhiteSpace(cs))
                {
                    optionsBuilder.UseSqlServer(cs);
                }
            }
        }
            //sınıfın ismi -  propun ismi   , ayrıca dbset<about> a ctrl . deyip entityLayerConcretei seçiyoruz     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(x => x.HomeTeam)
                .WithMany(y => y.HomeMatch)
                .HasForeignKey(z => z.HomeTeamID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Match>()
                .HasOne(x => x.GuestTeam)
                .WithMany(y => y.AwayMatch)
                .HasForeignKey(z => z.GuestTeamID)
                .OnDelete(DeleteBehavior.ClientSetNull);


            modelBuilder.Entity<Message2>()
                .HasOne(x => x.SenderUser)
                .WithMany(y => y.WriterSender)
                .HasForeignKey(z => z.SenderID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Message2>()
                .HasOne(x => x.ReceiverUser)
                .WithMany(y => y.WriterReceiver)
                .HasForeignKey(z => z.ReceiverID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Blog>()
                .ToTable(t => t.HasTrigger("AddBlogInRatingTable"));

            modelBuilder.Entity<Writer>()
                .HasOne(w => w.AppUser)
                .WithMany()
                .HasForeignKey(w => w.AppUserId);
           

            modelBuilder.Entity<Blog>()
                .Property(b => b.LikeCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<BlogLike>()
                .HasIndex(x => new { x.BlogID, x.AppUserId })
                .IsUnique();

            modelBuilder.Entity<BlogLike>()
                .HasOne(x => x.Blog)
                .WithMany()
                .HasForeignKey(x => x.BlogID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogLike>()
                .HasOne(x=>x.AppUser)
                .WithMany()
                .HasForeignKey(x=>x.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Comment>()
                .HasOne(x => x.Blog)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.BlogID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder); //hasnokey hatasını önlemek için yazdık
            //HomeMatch-->WriterSender
            //AwayMatch-->WriterReceiver

            //HomeTeam-->SenderUser
            //GuestTeam-->ReceiverUser
        }



        public DbSet<About> Abouts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogRating> BlogRatings { get; set; }
        public DbSet<BlogLike> BlogLikes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Message2> Message2s { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }
}
