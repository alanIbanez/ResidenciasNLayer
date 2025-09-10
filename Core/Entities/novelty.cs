namespace ResidenciasNLayer.Core.Entities;

public class novelty
{
    public int id { get; set; }
    public int resident_id { get; set; }
    public string title { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public DateTime created_at { get; set; }
    
    // Navigation property
    public resident resident { get; set; } = null!;
}