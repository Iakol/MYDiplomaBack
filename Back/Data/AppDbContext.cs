using Back.Data.FluentConfig;
using Back.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Back.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions option):base(option) {   }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ShopingCard> shopingCards { get; set; }
        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<BookComment> BookComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.ApplyConfiguration(new AuthorFluentConfig());
            modelBuilder.ApplyConfiguration(new BookCommentFluentConfig());
            modelBuilder.ApplyConfiguration(new BookFluentConfig());
            modelBuilder.ApplyConfiguration(new CardItemFluentConfig());
            modelBuilder.ApplyConfiguration(new OrderDetailFluentConfig());
            modelBuilder.ApplyConfiguration(new OrderHeaderFluentConfig());
            modelBuilder.ApplyConfiguration(new ShopingCardFluentConfig());
            modelBuilder.ApplyConfiguration(new SubjectFluentConfig());

        }



    }
}
