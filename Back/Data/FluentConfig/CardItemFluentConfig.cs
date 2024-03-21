using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class CardItemFluentConfig : IEntityTypeConfiguration<CardItem>
    {
        public void Configure(EntityTypeBuilder<CardItem> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(ci => ci.Id);

            // other validation
            //relationship
            modelBuilder.HasOne(ci => ci.Book).WithMany(b => b.CardItems).HasForeignKey(ci => ci.BookId);
            modelBuilder.HasOne(ci => ci.ShopingCard).WithMany(sc => sc.CardItems).HasForeignKey(ci => ci.ShopingCardId);




        }
    }
}
