using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class TimeSlotConflictConfiguration : IEntityTypeConfiguration<TimeSLotConflict>
    {
        public void Configure(EntityTypeBuilder<TimeSLotConflict> builder)
        {
            builder.Property(entity => entity.Id)
                 .UseIdentityColumn()
                 .IsRequired(true)
                 .HasColumnName("Id")
                 .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.SlotId).HasColumnName("SlotId").HasColumnType("int");

            builder.Property(entity => entity.ConflictSlotId)
                   .HasColumnName("ConflictSlotId")
                   .HasColumnType("int");

            builder.Property(entity => entity.Conflict)
                  .HasColumnName("Conflict")
                  .HasColumnType("bit");

            builder.Property(entity => entity.SemesterId).HasColumnName("SemesterId").HasColumnType("int");
        }
    }
}
