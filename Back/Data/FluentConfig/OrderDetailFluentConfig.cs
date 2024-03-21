using Back.Model;
using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class OrderDetailFluentConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(od => od.OrderDetailId);
            // other validation
            modelBuilder.Property(od => od.OrderHeaderId).IsRequired();
            modelBuilder.Property(od => od.BookId).IsRequired();
            modelBuilder.Property(od => od.Quantity).IsRequired();
            modelBuilder.Property(od => od.IteamName).IsRequired();
            modelBuilder.Property(od => od.Price).IsRequired();

            //relationship
            modelBuilder.HasOne(od=>od.Book).WithMany().HasForeignKey(od => od.BookId);




        }
    }
}
