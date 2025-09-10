namespace ResidenciasNLayer.Domain.Entities;

public class ExitRequest
{
    public int id { get; set; }
    public int residentid { get; set; }
    public int? tutorid { get; set; }
    public string reason { get; set; } = string.Empty;
    public DateTime requestdate { get; set; }
    public DateTime? exitdate { get; set; }
    public DateTime? returndate { get; set; }
    public string status { get; set; } = string.Empty; // pending, approved, rejected, exited, returned
    public string? approvedby { get; set; } // role of who approved
    public DateTime createdat { get; set; }
    public DateTime updatedat { get; set; }

    // Navigation properties
    public User resident { get; set; } = null!;
    public User? tutor { get; set; }
}