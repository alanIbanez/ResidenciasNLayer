namespace ResidenciasNLayer.Domain.Entities;

public class Guard
{
    public int Id { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    public int ShiftId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Shift Shift { get; set; } = null!;
}