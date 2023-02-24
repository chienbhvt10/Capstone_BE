using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class TimeSLotConfiguration : IEntityTypeConfiguration<TimeSlot>
    {
        public void Configure(EntityTypeBuilder<TimeSlot> builder)
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

            builder.Property(entity => entity.Description)
                   .HasColumnName("Description")
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50)
                    .HasDefaultValue(null)
                   .IsRequired(false);

            builder.Property(entity => entity.SemesterId).HasColumnName("SemesterId").HasColumnType("int");

            builder.Property(entity => entity.OrderNumber)
               .HasColumnName("OrderNumber")
               .HasColumnType("int");
        }
    }
}
