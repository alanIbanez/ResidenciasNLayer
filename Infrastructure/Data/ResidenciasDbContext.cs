using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Core.Entities;

namespace ResidenciasNLayer.Infrastructure.Data;

public class ResidenciasDbContext : DbContext
{
    public ResidenciasDbContext(DbContextOptions<ResidenciasDbContext> options) : base(options)
    {
    }

    public DbSet<user> users { get; set; }
    public DbSet<preceptortype> preceptortypes { get; set; }
    public DbSet<residenttype> residenttypes { get; set; }
    public DbSet<shift> shifts { get; set; }
    public DbSet<tutor> tutors { get; set; }
    public DbSet<preceptor> preceptors { get; set; }
    public DbSet<guard> guards { get; set; }
    public DbSet<resident> residents { get; set; }
    public DbSet<device> devices { get; set; }
    public DbSet<usertoken> usertokens { get; set; }
    public DbSet<exitrequest> exitrequests { get; set; }
    public DbSet<@event> events { get; set; }
    public DbSet<attendance> attendances { get; set; }
    public DbSet<novelty> novelties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names to be lowercase
        modelBuilder.Entity<user>().ToTable("user");
        modelBuilder.Entity<preceptortype>().ToTable("preceptortype");
        modelBuilder.Entity<residenttype>().ToTable("residenttype");
        modelBuilder.Entity<shift>().ToTable("shift");
        modelBuilder.Entity<tutor>().ToTable("tutor");
        modelBuilder.Entity<preceptor>().ToTable("preceptor");
        modelBuilder.Entity<guard>().ToTable("guard");
        modelBuilder.Entity<resident>().ToTable("resident");
        modelBuilder.Entity<device>().ToTable("device");
        modelBuilder.Entity<usertoken>().ToTable("usertoken");
        modelBuilder.Entity<exitrequest>().ToTable("exitrequest");
        modelBuilder.Entity<@event>().ToTable("event");
        modelBuilder.Entity<attendance>().ToTable("attendance");
        modelBuilder.Entity<novelty>().ToTable("novelty");

        // Configure unique constraints
        modelBuilder.Entity<user>()
            .HasIndex(u => u.username)
            .IsUnique();

        // Configure foreign key relationships
        modelBuilder.Entity<tutor>()
            .HasOne(t => t.user)
            .WithMany()
            .HasForeignKey(t => t.user_id);

        modelBuilder.Entity<preceptor>()
            .HasOne(p => p.user)
            .WithMany()
            .HasForeignKey(p => p.user_id);

        modelBuilder.Entity<preceptor>()
            .HasOne(p => p.preceptortype)
            .WithMany()
            .HasForeignKey(p => p.preceptortype_id);

        modelBuilder.Entity<preceptor>()
            .HasOne(p => p.shift)
            .WithMany()
            .HasForeignKey(p => p.shift_id);

        modelBuilder.Entity<guard>()
            .HasOne(g => g.user)
            .WithMany()
            .HasForeignKey(g => g.user_id);

        modelBuilder.Entity<guard>()
            .HasOne(g => g.shift)
            .WithMany()
            .HasForeignKey(g => g.shift_id);

        modelBuilder.Entity<resident>()
            .HasOne(r => r.user)
            .WithMany()
            .HasForeignKey(r => r.user_id);

        modelBuilder.Entity<resident>()
            .HasOne(r => r.residenttype)
            .WithMany()
            .HasForeignKey(r => r.residenttype_id);

        modelBuilder.Entity<resident>()
            .HasOne(r => r.tutor)
            .WithMany()
            .HasForeignKey(r => r.tutor_id);

        modelBuilder.Entity<device>()
            .HasOne(d => d.user)
            .WithMany()
            .HasForeignKey(d => d.user_id);

        modelBuilder.Entity<usertoken>()
            .HasOne(ut => ut.user)
            .WithMany()
            .HasForeignKey(ut => ut.user_id);

        modelBuilder.Entity<exitrequest>()
            .HasOne(er => er.resident)
            .WithMany()
            .HasForeignKey(er => er.resident_id);

        modelBuilder.Entity<attendance>()
            .HasOne(a => a.resident)
            .WithMany()
            .HasForeignKey(a => a.resident_id);

        modelBuilder.Entity<attendance>()
            .HasOne(a => a.@event)
            .WithMany()
            .HasForeignKey(a => a.event_id);

        modelBuilder.Entity<novelty>()
            .HasOne(n => n.resident)
            .WithMany()
            .HasForeignKey(n => n.resident_id);

        // Configure default values
        modelBuilder.Entity<device>()
            .Property(d => d.active)
            .HasDefaultValue(true);

        modelBuilder.Entity<device>()
            .Property(d => d.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<usertoken>()
            .Property(ut => ut.issued_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<novelty>()
            .Property(n => n.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}