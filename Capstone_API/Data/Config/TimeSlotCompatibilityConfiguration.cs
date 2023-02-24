using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class TimeSlotCompatibilityConfiguration : IEntityTypeConfiguration<TimeSlotCompatibility>
    {


        public void Configure(EntityTypeBuilder<TimeSlotCompatibility> builder)
        {
            builder.Property(entity => entity.Id)
                 .UseIdentityColumn()
                 .IsRequired(true)
                 .HasColumnName("Id")
                 .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.SlotId).HasColumnName("SlotId").HasColumnType("int");

            builder.Property(entity => entity.CompatibilitySlotId).HasColumnName("CompatibilitySlotId").HasColumnType("int");

            builder.Property(entity => entity.CompatibilityLevel).HasColumnName("CompatibilityLevel").HasColumnType("int");

            builder.Property(entity => entity.SemesterId)
                .HasColumnName("SemesterId")
                .HasColumnType("int");
        }
    }
}
