using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class BookFluentConfig: IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(b => b.Id);
            // other validation
            modelBuilder.Property(b => b.Name).IsRequired();
            modelBuilder.Property(b => b.ISBN).IsRequired();

            //relationship
            modelBuilder.HasOne(b => b.RelatedBook).WithOne().HasForeignKey<Book>(b => b.RelatedBookId);


        }
    }
}
