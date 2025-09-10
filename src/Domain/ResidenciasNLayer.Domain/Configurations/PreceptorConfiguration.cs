using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class PreceptorConfiguration : IEntityTypeConfiguration<Preceptor>
{
    public void Configure(EntityTypeBuilder<Preceptor> builder)
    {
        builder.ToTable("preceptor");
        
        builder.HasKey(p => p.Id);
        
        // Foreign key relationships
        builder.HasOne(p => p.User)
            .WithOne(u => u.Preceptor)
            .HasForeignKey<Preceptor>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.PreceptorType)
            .WithMany(pt => pt.Preceptors)
            .HasForeignKey(p => p.PreceptorTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.Shift)
            .WithMany(s => s.Preceptors)
            .HasForeignKey(p => p.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}