using Capstone_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone_API.Data.Config
{
    public class DistanceConfiguration : IEntityTypeConfiguration<Distance>
    {
        public void Configure(EntityTypeBuilder<Distance> builder)
        {
            builder.Property(entity => entity.Id)
                   .UseIdentityColumn()
                   .IsRequired(true)
                   .HasColumnName("Id")
                   .HasColumnType("int");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Building1Id)
                    .HasColumnName("Building1Id")
                    .HasColumnType("int");

            builder.Property(entity => entity.Building2Id)
                   .HasColumnName("Building2Id")
                   .HasColumnType("int");

            builder.Property(entity => entity.DistanceBetween)
                   .HasColumnName("DistanceBetween")
                   .HasColumnType("int");
        }
    }
}
