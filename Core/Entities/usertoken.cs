namespace ResidenciasNLayer.Core.Entities;

public class usertoken
{
    public int id { get; set; }
    public int user_id { get; set; }
    public string token { get; set; } = string.Empty;
    public DateTime issued_at { get; set; }
    
    // Navigation property
    public user user { get; set; } = null!;
}