namespace ResidenciasNLayer.Domain.Entities;

public class User
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public string? expotoken { get; set; }
    
    // Navigation property
    public ICollection<Device> devices { get; set; } = new List<Device>();
}
