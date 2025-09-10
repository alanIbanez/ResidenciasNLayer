namespace ResidenciasNLayer.Domain.Entities;

public class ExitStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    // Navigation properties
    public virtual ICollection<Exit> Exits { get; set; } = new List<Exit>();
}