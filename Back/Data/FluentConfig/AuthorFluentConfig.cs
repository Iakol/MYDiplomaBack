using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Back.Data.FluentConfig
{
    public class AuthorFluentConfig: IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(a => a.AuthorId);
            // other validation
            modelBuilder.Property(a => a.FirstName).IsRequired();
            modelBuilder.Property(a => a.SecondName).IsRequired();

            //Relationship



        }
    }
}
