using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class SlotDayConfiguration : IEntityTypeConfiguration<SlotDay>
    {
        public void Configure(EntityTypeBuilder<SlotDay> builder)
        {
            builder.Property(entity => entity.Id)
                  .UseIdentityColumn()
                  .IsRequired(true)
                  .HasColumnName("Id")
                  .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.NumberOfSlots)
                    .HasColumnName("NumberOfSlots")
                    .HasColumnType("int");

            builder.Property(entity => entity.SemesterId).HasColumnName("SemesterId").HasColumnType("int");


        }
    }
}
