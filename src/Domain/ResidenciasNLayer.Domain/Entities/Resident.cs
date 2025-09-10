namespace ResidenciasNLayer.Domain.Entities;

public class Resident
{
    public int Id { get; set; }
    public string? StudentId { get; set; }
    public string? Institution { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    public int ResidentTypeId { get; set; }
    public int TutorId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ResidentType ResidentType { get; set; } = null!;
    public virtual Tutor Tutor { get; set; } = null!;
    public virtual ICollection<Exit> Exits { get; set; } = new List<Exit>();
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}