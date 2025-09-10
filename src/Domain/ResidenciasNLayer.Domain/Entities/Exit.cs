namespace ResidenciasNLayer.Domain.Entities;

public class Exit
{
    public int Id { get; set; }
    public DateTime PlannedDepartureAt { get; set; }
    public DateTime PlannedReturnAt { get; set; }
    public DateTime? ActualDepartureAt { get; set; }
    public DateTime? ActualReturnAt { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign keys
    public int ResidentId { get; set; }
    public int ExitTypeId { get; set; }
    public int ExitStatusId { get; set; }
    
    // Navigation properties
    public virtual Resident Resident { get; set; } = null!;
    public virtual ExitType ExitType { get; set; } = null!;
    public virtual ExitStatus ExitStatus { get; set; } = null!;
    public virtual ICollection<ExitAuthorization> Authorizations { get; set; } = new List<ExitAuthorization>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}