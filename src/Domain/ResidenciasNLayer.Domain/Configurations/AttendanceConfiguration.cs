using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("attendance");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.OccurredAt)
            .IsRequired();
        
        builder.Property(a => a.Type)
            .HasMaxLength(50);
        
        builder.Property(a => a.RegisteredAt)
            .IsRequired()
            .HasDefaultValueSql("now()");
        
        builder.Property(a => a.Notes)
            .HasMaxLength(1000);
        
        // Foreign key relationships
        builder.HasOne(a => a.Resident)
            .WithMany(r => r.Attendances)
            .HasForeignKey(a => a.ResidentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes for efficient querying
        builder.HasIndex(a => new { a.ResidentId, a.OccurredAt });
        builder.HasIndex(a => a.Type);
    }
}