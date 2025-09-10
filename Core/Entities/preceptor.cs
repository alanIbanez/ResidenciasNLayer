namespace ResidenciasNLayer.Core.Entities;

public class preceptor
{
    public int id { get; set; }
    public int user_id { get; set; }
    public int preceptortype_id { get; set; }
    public int shift_id { get; set; }
    
    // Navigation properties
    public user user { get; set; } = null!;
    public preceptortype preceptortype { get; set; } = null!;
    public shift shift { get; set; } = null!;
}