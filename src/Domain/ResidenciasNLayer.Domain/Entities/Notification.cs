namespace ResidenciasNLayer.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string Type { get; set; } = null!; // exit_requested, exit_approved, exit_rejected, exit_canceled, guard_departure, guard_return, event_published
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public string? Status { get; set; } // pending, sent, delivered, failed
    public string? ProviderMessageId { get; set; }
    public string? ProviderResponse { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    public int? ExitId { get; set; }
    public int? EventId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Exit? Exit { get; set; }
    // Note: Event entity would be created if needed for events functionality
}