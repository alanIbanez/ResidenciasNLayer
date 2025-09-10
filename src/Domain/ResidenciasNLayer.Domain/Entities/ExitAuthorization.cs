namespace ResidenciasNLayer.Domain.Entities;

public class ExitAuthorization
{
    public int Id { get; set; }
    public string Action { get; set; } = null!; // requested, approved, rejected, canceled, guard_departure, guard_return
    public string PerformedByRole { get; set; } = null!; // Preceptor, Tutor, Guardia, Residente
    public string? Reason { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    
    // Foreign keys
    public int ExitId { get; set; }
    public int PerformedByUserId { get; set; }
    
    // Navigation properties
    public virtual Exit Exit { get; set; } = null!;
    public virtual User PerformedByUser { get; set; } = null!;
}