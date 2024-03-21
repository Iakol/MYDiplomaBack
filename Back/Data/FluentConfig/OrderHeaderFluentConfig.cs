using Back.Model;
using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class OrderHeaderFluentConfig : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(oh => oh.OrderHeaderId);

            // other validation
            modelBuilder.Property(oh => oh.PickUpName).IsRequired();
            modelBuilder.Property(oh => oh.PickUpPhone).IsRequired();
            modelBuilder.Property(oh => oh.PickUpEmail).IsRequired();

            //relationship
            modelBuilder.HasOne(oh => oh.user).WithMany().HasForeignKey(oh => oh.UserId);



        }
    }
}
