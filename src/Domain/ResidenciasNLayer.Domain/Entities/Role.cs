namespace ResidenciasNLayer.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}