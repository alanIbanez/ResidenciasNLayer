using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Domain.Configurations;

public class ExitStatusConfiguration : IEntityTypeConfiguration<ExitStatus>
{
    public void Configure(EntityTypeBuilder<ExitStatus> builder)
    {
        builder.ToTable("exitstatus");
        
        builder.HasKey(es => es.Id);
        
        builder.Property(es => es.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(es => es.Name)
            .IsUnique();
    }
}