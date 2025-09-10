using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Entities;

namespace ResidenciasNLayer.Data;

/// <summary>
/// Database context for ResidenciasNLayer application
/// </summary>
public class ResidenciasDbContext : DbContext
{
    public ResidenciasDbContext(DbContextOptions<ResidenciasDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Devices DbSet
    /// </summary>
    public DbSet<Device> Devices { get; set; }

    /// <summary>
    /// Users DbSet
    /// </summary>
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Device entity
        modelBuilder.Entity<Device>(entity =>
        {
            // Map to lowercase table name
            entity.ToTable("device");

            // Configure deviceid as primary key
            entity.HasKey(e => e.deviceid);

            // Configure properties with lowercase column names
            entity.Property(e => e.deviceid)
                .HasColumnName("deviceid")
                .IsRequired();

            entity.Property(e => e.tokenfcm)
                .HasColumnName("tokenfcm")
                .IsRequired();

            entity.Property(e => e.estado)
                .HasColumnName("estado")
                .HasDefaultValue(true);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Map to lowercase table name
            entity.ToTable("user");

            // Configure userid as primary key
            entity.HasKey(e => e.userid);

            // Configure properties with lowercase column names
            entity.Property(e => e.userid)
                .HasColumnName("userid")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.username)
                .HasColumnName("username")
                .IsRequired();

            entity.Property(e => e.email)
                .HasColumnName("email")
                .IsRequired();

            entity.Property(e => e.deviceid)
                .HasColumnName("deviceid");

            // Configure relationship: User -> Device (1:1)
            entity.HasOne(u => u.Device)
                .WithOne(d => d.User)
                .HasForeignKey<User>(u => u.deviceid)
                .HasPrincipalKey<Device>(d => d.deviceid)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
