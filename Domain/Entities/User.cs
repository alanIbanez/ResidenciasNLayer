namespace ResidenciasNLayer.Domain.Entities;

public class User
{
    public int id { get; set; }
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public string? expotoken { get; set; }
    public DateTime createdat { get; set; }
    public DateTime updatedat { get; set; }

    // Navigation properties
    public ICollection<UserToken> usertokens { get; set; } = new List<UserToken>();
    public ICollection<Event> events { get; set; } = new List<Event>();
    public ICollection<ExitRequest> exitrequests { get; set; } = new List<ExitRequest>();
}