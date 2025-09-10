using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Domain.Configurations;

namespace ResidenciasNLayer.Infrastructure.Data;

public class ResidenciasDbContext : DbContext
{
    public ResidenciasDbContext(DbContextOptions<ResidenciasDbContext> options) : base(options)
    {
    }

    // Lookup entities
    public DbSet<Role> Roles { get; set; }
    public DbSet<PreceptorType> PreceptorTypes { get; set; }
    public DbSet<ResidentType> ResidentTypes { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ExitType> ExitTypes { get; set; }
    public DbSet<ExitStatus> ExitStatuses { get; set; }

    // Main entities
    public DbSet<User> Users { get; set; }
    public DbSet<Preceptor> Preceptors { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Guard> Guards { get; set; }
    public DbSet<Resident> Residents { get; set; }
    public DbSet<Exit> Exits { get; set; }
    public DbSet<ExitAuthorization> ExitAuthorizations { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PreceptorTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResidentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ShiftConfiguration());
        modelBuilder.ApplyConfiguration(new ExitTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ExitStatusConfiguration());
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PreceptorConfiguration());
        modelBuilder.ApplyConfiguration(new TutorConfiguration());
        modelBuilder.ApplyConfiguration(new GuardConfiguration());
        modelBuilder.ApplyConfiguration(new ResidentConfiguration());
        modelBuilder.ApplyConfiguration(new ExitConfiguration());
        modelBuilder.ApplyConfiguration(new ExitAuthorizationConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
    }
}