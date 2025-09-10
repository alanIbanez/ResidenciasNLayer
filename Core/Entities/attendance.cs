namespace ResidenciasNLayer.Core.Entities;

public class attendance
{
    public int id { get; set; }
    public int resident_id { get; set; }
    public string type { get; set; } = string.Empty; // "diaria"|"evento"
    public DateTime date_at { get; set; }
    public int? event_id { get; set; }
    
    // Navigation properties
    public resident resident { get; set; } = null!;
    public @event? @event { get; set; }
}