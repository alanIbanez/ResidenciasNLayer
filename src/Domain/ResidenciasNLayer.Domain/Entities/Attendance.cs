namespace ResidenciasNLayer.Domain.Entities;

public class Attendance
{
    public int Id { get; set; }
    public DateTime OccurredAt { get; set; }
    public string? Type { get; set; } // Simple string type for now, could be FK to AttendanceType if needed
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    
    // Foreign keys
    public int ResidentId { get; set; }
    public int? EventId { get; set; } // Optional reference to an event
    
    // Navigation properties
    public virtual Resident Resident { get; set; } = null!;
    // Note: Event entity would be created if needed for events functionality
}