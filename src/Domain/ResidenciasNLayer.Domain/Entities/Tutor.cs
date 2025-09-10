namespace ResidenciasNLayer.Domain.Entities;

public class Tutor
{
    public int Id { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Resident> Residents { get; set; } = new List<Resident>();
}