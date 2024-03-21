using Back.Model;
using Back.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Data.FluentConfig
{
    public class SubjectFluentConfig: IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> modelBuilder) {
            //name of table
            //name of colums
            //Primary Keys
            modelBuilder.HasKey(s => s.SubjectId);

            // other validation
            modelBuilder.Property(s => s.SubjectName).IsRequired();
            //relationship




        }
    }
}
