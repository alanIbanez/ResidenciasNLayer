using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> users { get; set; }
    public DbSet<Device> devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.username).HasColumnName("username");
            entity.Property(e => e.email).HasColumnName("email");
            entity.Property(e => e.expotoken).HasColumnName("expotoken");
        });

        // Configure Device entity
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("device");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.deviceid).HasColumnName("deviceid").IsRequired();
            entity.Property(e => e.tokenfcm).HasColumnName("tokenfcm").HasColumnType("text");
            entity.Property(e => e.user_id).HasColumnName("user_id");

            // Configure relationship
            entity.HasOne(d => d.user)
                  .WithMany(u => u.devices)
                  .HasForeignKey(d => d.user_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure unique index on (user_id, deviceid)
            entity.HasIndex(e => new { e.user_id, e.deviceid })
                  .IsUnique()
                  .HasDatabaseName("IX_device_user_id_deviceid");
        });
    }
}
