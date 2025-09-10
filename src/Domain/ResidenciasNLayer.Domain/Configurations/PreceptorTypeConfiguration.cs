using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class PreceptorTypeConfiguration : IEntityTypeConfiguration<PreceptorType>
{
    public void Configure(EntityTypeBuilder<PreceptorType> builder)
    {
        builder.ToTable("preceptortype");
        
        builder.HasKey(pt => pt.Id);
        
        builder.Property(pt => pt.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(pt => pt.Name)
            .IsUnique();
    }
}