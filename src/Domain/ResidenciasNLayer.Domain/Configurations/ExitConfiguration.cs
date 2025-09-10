using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ExitConfiguration : IEntityTypeConfiguration<Exit>
{
    public void Configure(EntityTypeBuilder<Exit> builder)
    {
        builder.ToTable("exit");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Notes)
            .HasMaxLength(1000);
        
        builder.Property(e => e.Reason)
            .HasMaxLength(500);
        
        // Foreign key relationships
        builder.HasOne(e => e.Resident)
            .WithMany(r => r.Exits)
            .HasForeignKey(e => e.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.ExitType)
            .WithMany(et => et.Exits)
            .HasForeignKey(e => e.ExitTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.ExitStatus)
            .WithMany(es => es.Exits)
            .HasForeignKey(e => e.ExitStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}