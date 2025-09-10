using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("shift");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(s => s.Name)
            .IsUnique();
    }
}