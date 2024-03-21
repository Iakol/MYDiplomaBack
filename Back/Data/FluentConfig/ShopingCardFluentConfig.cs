using Back.Model;
using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class ShopingCardFluentConfig : IEntityTypeConfiguration<ShopingCard>
    {
        public void Configure(EntityTypeBuilder<ShopingCard> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(sc => sc.Id);

            // other validation
            modelBuilder.Ignore(sc => sc.StripePaymantId);
            modelBuilder.Ignore(sc => sc.ClientSecret);
            modelBuilder.Ignore(sc => sc.CartTotal);

            //relationship
            modelBuilder.HasOne(sc => sc.User).WithOne().HasForeignKey<ShopingCard>(sc => sc.UserId);




        }
    }
}
