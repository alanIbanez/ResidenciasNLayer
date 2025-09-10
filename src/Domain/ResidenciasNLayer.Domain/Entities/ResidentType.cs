namespace ResidenciasNLayer.Domain.Entities;

public class ResidentType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    // Navigation properties
    public virtual ICollection<Resident> Residents { get; set; } = new List<Resident>();
}