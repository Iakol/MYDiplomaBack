using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class BookCommentFluentConfig : IEntityTypeConfiguration<BookComment>
    {
        public void Configure(EntityTypeBuilder<BookComment> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(bc => bc.BookCommentId);
            // other validation
            modelBuilder.Property(bc => bc.Text).IsRequired();
            modelBuilder.Property(bc => bc.Raiting).IsRequired();

            //relationship
            modelBuilder.HasOne(bc => bc.Book).WithMany(b => b.BookComments).HasForeignKey(bc => bc.BookId);
            modelBuilder.HasOne(bc => bc.User).WithMany(u => u.BookComments).HasForeignKey(bc => bc.UserId);





        }
    }
}
