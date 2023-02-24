using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class SubjectPreferenceLevelConfiguration : IEntityTypeConfiguration<SubjectPreferenceLevel>
    {
        public void Configure(EntityTypeBuilder<SubjectPreferenceLevel> builder)
        {
            builder.Property(entity => entity.Id)
                 .UseIdentityColumn()
                 .IsRequired(true)
                 .HasColumnName("Id")
                 .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.LecturerId)
               .HasColumnName("LecturerId")
               .HasColumnType("int");

            builder.Property(entity => entity.SubjectId).HasColumnName("SubjectId").HasColumnType("int");

            builder.Property(entity => entity.PreferenceLevel).HasColumnName("PreferenceLevel").HasColumnType("int");

            builder.Property(entity => entity.SemesterId).HasColumnName("SemesterId").HasColumnType("int");

        }
    }
}
