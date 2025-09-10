namespace ResidenciasNLayer.Domain.Entities;

public class Preceptor
{
    public int Id { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    public int PreceptorTypeId { get; set; }
    public int ShiftId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual PreceptorType PreceptorType { get; set; } = null!;
    public virtual Shift Shift { get; set; } = null!;
}