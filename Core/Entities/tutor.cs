namespace ResidenciasNLayer.Core.Entities;

public class tutor
{
    public int id { get; set; }
    public int user_id { get; set; }
    
    // Navigation property
    public user user { get; set; } = null!;
}