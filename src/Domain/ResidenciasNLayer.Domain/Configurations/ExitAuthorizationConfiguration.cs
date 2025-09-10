using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ExitAuthorizationConfiguration : IEntityTypeConfiguration<ExitAuthorization>
{
    public void Configure(EntityTypeBuilder<ExitAuthorization> builder)
    {
        builder.ToTable("exitauthorization");
        
        builder.HasKey(ea => ea.Id);
        
        builder.Property(ea => ea.Action)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(ea => ea.PerformedByRole)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(ea => ea.Reason)
            .HasMaxLength(500);
        
        builder.Property(ea => ea.Notes)
            .HasMaxLength(1000);
        
        // Foreign key relationships
        builder.HasOne(ea => ea.Exit)
            .WithMany(e => e.Authorizations)
            .HasForeignKey(ea => ea.ExitId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ea => ea.PerformedByUser)
            .WithMany(u => u.ExitAuthorizations)
            .HasForeignKey(ea => ea.PerformedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Index for efficient querying
        builder.HasIndex(ea => new { ea.ExitId, ea.Action });
    }
}