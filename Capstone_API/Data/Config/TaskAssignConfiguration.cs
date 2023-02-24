using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class TaskAssignConfiguration : IEntityTypeConfiguration<TaskAssign>
    {
        public void Configure(EntityTypeBuilder<TaskAssign> builder)
        {
            builder.Property(entity => entity.Id)
                 .UseIdentityColumn()
                 .IsRequired(true)
                 .HasColumnName("Id")
                 .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.ClassId).HasColumnName("ClassId").HasColumnType("int");

            builder.Property(entity => entity.TimeSlotId).HasColumnName("TimeSlotId").HasColumnType("int");

            builder.Property(entity => entity.SubjectId).HasColumnName("SubjectId").HasColumnType("int");

            builder.Property(entity => entity.SemesterId).HasColumnName("SemesterId").HasColumnType("int");

            builder.Property(entity => entity.BuildingId).HasColumnName("SemesterId").HasColumnType("int");

            builder.Property(entity => entity.Department)
                    .HasColumnName("Department")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.Slot1)
                 .HasColumnName("Slot1")
                 .HasColumnType("nvarchar")
                 .HasMaxLength(50)
                 .HasDefaultValue(null)
                 .IsRequired(false);

            builder.Property(entity => entity.Slot2)
                    .HasColumnName("Slot2")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.Room1)
                    .HasColumnName("Slot2")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.Room2)
                    .HasColumnName("Slot2")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.Status)
             .HasColumnName("Status")
             .HasColumnType("bit");
        }
    }
}
