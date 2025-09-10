using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ResidentConfiguration : IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        builder.ToTable("resident");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.StudentId)
            .HasMaxLength(50);
        
        builder.Property(r => r.Institution)
            .HasMaxLength(200);
        
        // Foreign key relationships
        builder.HasOne(r => r.User)
            .WithOne(u => u.Resident)
            .HasForeignKey<Resident>(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(r => r.ResidentType)
            .WithMany(rt => rt.Residents)
            .HasForeignKey(r => r.ResidentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Tutor)
            .WithMany(t => t.Residents)
            .HasForeignKey(r => r.TutorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(r => r.StudentId)
            .IsUnique()
            .HasFilter("\"StudentId\" IS NOT NULL");
    }
}