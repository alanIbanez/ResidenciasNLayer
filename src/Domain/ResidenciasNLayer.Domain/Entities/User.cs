namespace ResidenciasNLayer.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PushToken { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign keys
    public int RoleId { get; set; }
    
    // Navigation properties
    public virtual Role Role { get; set; } = null!;
    public virtual Preceptor? Preceptor { get; set; }
    public virtual Tutor? Tutor { get; set; }
    public virtual Guard? Guard { get; set; }
    public virtual Resident? Resident { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<ExitAuthorization> ExitAuthorizations { get; set; } = new List<ExitAuthorization>();
}