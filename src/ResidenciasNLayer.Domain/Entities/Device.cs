namespace ResidenciasNLayer.Domain.Entities;

public class Device
{
    public int id { get; set; }
    public string deviceid { get; set; } = string.Empty;
    public string tokenfcm { get; set; } = string.Empty;
    public int user_id { get; set; }
    
    // Navigation property
    public User user { get; set; } = null!;
}