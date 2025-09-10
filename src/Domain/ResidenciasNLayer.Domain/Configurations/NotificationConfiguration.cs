using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notification");
        
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Type)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(n => n.Body)
            .IsRequired()
            .HasMaxLength(1000);
        
        builder.Property(n => n.Status)
            .HasMaxLength(50);
        
        builder.Property(n => n.ProviderMessageId)
            .HasMaxLength(200);
        
        // Foreign key relationships
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(n => n.Exit)
            .WithMany(e => e.Notifications)
            .HasForeignKey(n => n.ExitId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes for efficient querying
        builder.HasIndex(n => new { n.UserId, n.IsRead });
        builder.HasIndex(n => n.Type);
    }
}