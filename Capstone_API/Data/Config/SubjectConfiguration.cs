using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.Property(entity => entity.Id)
                  .UseIdentityColumn()
                  .IsRequired(true)
                  .HasColumnName("Id")
                  .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Code)
                    .HasColumnName("Code")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.Name)
                   .HasColumnName("Name")
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50)
                    .HasDefaultValue(null)
                   .IsRequired(false);

            builder.Property(entity => entity.Department)
                  .HasColumnName("Department")
                  .HasColumnType("nvarchar")
                  .HasMaxLength(50)
                   .HasDefaultValue(null)
                  .IsRequired(false);


            builder.Property(entity => entity.SemesterId)
               .HasColumnName("SemesterId")
               .HasColumnType("int");

            builder.Property(entity => entity.OrderNumber).HasColumnName("OrderNumber").HasColumnType("int");

        }
    }
}
