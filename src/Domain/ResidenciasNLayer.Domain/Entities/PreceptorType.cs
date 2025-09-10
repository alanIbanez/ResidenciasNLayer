namespace ResidenciasNLayer.Domain.Entities;

public class PreceptorType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    // Navigation properties
    public virtual ICollection<Preceptor> Preceptors { get; set; } = new List<Preceptor>();
}