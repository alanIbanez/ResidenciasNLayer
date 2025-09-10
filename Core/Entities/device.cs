namespace ResidenciasNLayer.Core.Entities;

public class device
{
    public int id { get; set; }
    public int user_id { get; set; }
    public string tokenfcm { get; set; } = string.Empty;
    public bool active { get; set; } = true;
    public DateTime created_at { get; set; }
    
    // Navigation property
    public user user { get; set; } = null!;
}