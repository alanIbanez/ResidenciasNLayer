namespace ResidenciasNLayer.Domain.Entities;

public class UserToken
{
    public int id { get; set; }
    public int userid { get; set; }
    public string token { get; set; } = string.Empty;
    public DateTime expiry { get; set; }
    public DateTime createdat { get; set; }

    // Navigation property
    public User user { get; set; } = null!;
}