using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.ToTable("tutor");
        
        builder.HasKey(t => t.Id);
        
        // Foreign key relationship
        builder.HasOne(t => t.User)
            .WithOne(u => u.Tutor)
            .HasForeignKey<Tutor>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}