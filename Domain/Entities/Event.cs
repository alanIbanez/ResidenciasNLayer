namespace ResidenciasNLayer.Domain.Entities;

public class Event
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public DateTime date_at { get; set; }
    public int created_by { get; set; }
    public DateTime createdat { get; set; }
    public DateTime updatedat { get; set; }

    // Navigation property
    public User createdby { get; set; } = null!;
}