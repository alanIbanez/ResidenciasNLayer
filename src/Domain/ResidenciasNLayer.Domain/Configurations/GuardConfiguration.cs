using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class GuardConfiguration : IEntityTypeConfiguration<Guard>
{
    public void Configure(EntityTypeBuilder<Guard> builder)
    {
        builder.ToTable("guard");
        
        builder.HasKey(g => g.Id);
        
        // Foreign key relationships
        builder.HasOne(g => g.User)
            .WithOne(u => u.Guard)
            .HasForeignKey<Guard>(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(g => g.Shift)
            .WithMany(s => s.Guards)
            .HasForeignKey(g => g.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}