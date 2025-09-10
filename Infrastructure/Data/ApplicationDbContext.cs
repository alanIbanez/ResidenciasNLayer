using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> users { get; set; }
    public DbSet<UserToken> usertokens { get; set; }
    public DbSet<Event> events { get; set; }
    public DbSet<ExitRequest> exitrequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.email).HasColumnName("email").IsRequired();
            entity.Property(e => e.password).HasColumnName("password").IsRequired();
            entity.Property(e => e.firstname).HasColumnName("firstname").IsRequired();
            entity.Property(e => e.lastname).HasColumnName("lastname").IsRequired();
            entity.Property(e => e.role).HasColumnName("role").IsRequired();
            entity.Property(e => e.expotoken).HasColumnName("expotoken").HasColumnType("TEXT");
            entity.Property(e => e.createdat).HasColumnName("createdat");
            entity.Property(e => e.updatedat).HasColumnName("updatedat");

            entity.HasIndex(e => e.email).IsUnique();
        });

        // UserToken entity configuration
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("usertoken");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.userid).HasColumnName("userid");
            entity.Property(e => e.token).HasColumnName("token").IsRequired();
            entity.Property(e => e.expiry).HasColumnName("expiry");
            entity.Property(e => e.createdat).HasColumnName("createdat");

            entity.HasOne(e => e.user)
                  .WithMany(u => u.usertokens)
                  .HasForeignKey(e => e.userid);
        });

        // Event entity configuration
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("event");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.name).HasColumnName("name").IsRequired();
            entity.Property(e => e.description).HasColumnName("description");
            entity.Property(e => e.date_at).HasColumnName("date_at");
            entity.Property(e => e.created_by).HasColumnName("created_by");
            entity.Property(e => e.createdat).HasColumnName("createdat");
            entity.Property(e => e.updatedat).HasColumnName("updatedat");

            entity.HasOne(e => e.createdby)
                  .WithMany(u => u.events)
                  .HasForeignKey(e => e.created_by);
        });

        // ExitRequest entity configuration
        modelBuilder.Entity<ExitRequest>(entity =>
        {
            entity.ToTable("exitrequest");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.residentid).HasColumnName("residentid");
            entity.Property(e => e.tutorid).HasColumnName("tutorid");
            entity.Property(e => e.reason).HasColumnName("reason").IsRequired();
            entity.Property(e => e.requestdate).HasColumnName("requestdate");
            entity.Property(e => e.exitdate).HasColumnName("exitdate");
            entity.Property(e => e.returndate).HasColumnName("returndate");
            entity.Property(e => e.status).HasColumnName("status").IsRequired();
            entity.Property(e => e.approvedby).HasColumnName("approvedby");
            entity.Property(e => e.createdat).HasColumnName("createdat");
            entity.Property(e => e.updatedat).HasColumnName("updatedat");

            entity.HasOne(e => e.resident)
                  .WithMany(u => u.exitrequests)
                  .HasForeignKey(e => e.residentid);

            entity.HasOne(e => e.tutor)
                  .WithMany()
                  .HasForeignKey(e => e.tutorid);
        });
    }
}