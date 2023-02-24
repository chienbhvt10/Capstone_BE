using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.Property(entity => entity.Id)
                  .UseIdentityColumn()
                  .IsRequired(true)
                  .HasColumnName("Id")
                  .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Solver)
                    .HasColumnName("Solver")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .HasDefaultValue(null)
                    .IsRequired(false);

            builder.Property(entity => entity.Strategy)
                   .HasColumnName("Strategy")
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50)
                    .HasDefaultValue(null)
                   .IsRequired(false);

            builder.Property(entity => entity.InputType)
              .HasColumnName("InputType")
              .HasColumnType("nvarchar")
              .HasMaxLength(50)
              .IsRequired(false);

            builder.Property(entity => entity.PriorityMovingDistanceSettingLevel)
               .HasColumnName("PriorityMovingDistanceSettingLevel")
               .HasColumnType("int");

            builder.Property(entity => entity.MinimizeCostOfTimeSettingLevel)
               .HasColumnName("MinimizeCostOfTimeSettingLevel")
               .HasColumnType("int");

            builder.Property(entity => entity.MinimizeNumberOfSubjectsSettingLevel)
              .HasColumnName("MinimizeNumberOfSubjectsSettingLevel")
              .HasColumnType("int");

            builder.Property(entity => entity.QuotaOfClassSettingLevel)
                .HasColumnName("QuotaOfClassSettingLevel")
                .HasColumnType("int");

            builder.Property(entity => entity.PreferenceLevelOfSlotSettingLevel)
                .HasColumnName("PreferenceLevelOfSlotSettingLevel")
                .HasColumnType("int");

            builder.Property(entity => entity.PreferenceLevelOfSubjectSettingLevel)
                .HasColumnName("QuotaOfClassSettingLevel")
                .HasColumnType("int");
        }
    }
}
