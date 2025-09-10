using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ResidentTypeConfiguration : IEntityTypeConfiguration<ResidentType>
{
    public void Configure(EntityTypeBuilder<ResidentType> builder)
    {
        builder.ToTable("residenttype");
        
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(rt => rt.Name)
            .IsUnique();
    }
}