using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ExitTypeConfiguration : IEntityTypeConfiguration<ExitType>
{
    public void Configure(EntityTypeBuilder<ExitType> builder)
    {
        builder.ToTable("exittype");
        
        builder.HasKey(et => et.Id);
        
        builder.Property(et => et.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(et => et.Name)
            .IsUnique();
    }
}