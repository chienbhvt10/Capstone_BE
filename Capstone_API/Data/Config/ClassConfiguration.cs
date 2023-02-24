using Exercise.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exercise.Data.Config
{
    internal class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
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
        }
    }
}