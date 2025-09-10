namespace ResidenciasNLayer.Core.Entities;

public class resident
{
    public int id { get; set; }
    public int user_id { get; set; }
    public int residenttype_id { get; set; }
    public int tutor_id { get; set; }
    
    // Navigation properties
    public user user { get; set; } = null!;
    public residenttype residenttype { get; set; } = null!;
    public tutor tutor { get; set; } = null!;
}