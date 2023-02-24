using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class LecturerConfiguration : IEntityTypeConfiguration<Lecturer>
    {
        public void Configure(EntityTypeBuilder<Lecturer> builder)
        {
            builder.Property(entity => entity.Id)
                   .UseIdentityColumn()
                   .IsRequired(true)
                   .HasColumnName("Id")
                   .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Name)
                    .HasColumnName("Name")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.ShortName)
                   .HasColumnName("ShortName")
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50)
                    .HasDefaultValue(null)
                   .IsRequired(false);

            builder.Property(entity => entity.SemesterId)
                   .HasColumnName("SemesterId")
                   .HasColumnType("int");

            builder.Property(entity => entity.OrderNumber)
               .HasColumnName("OrderNumber")
               .HasColumnType("int");
        }
    }
}
