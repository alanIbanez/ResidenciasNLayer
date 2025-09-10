namespace ResidenciasNLayer.Domain.Entities;

public class Shift
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    
    // Navigation properties
    public virtual ICollection<Preceptor> Preceptors { get; set; } = new List<Preceptor>();
    public virtual ICollection<Guard> Guards { get; set; } = new List<Guard>();
}