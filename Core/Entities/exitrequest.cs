namespace ResidenciasNLayer.Core.Entities;

public class exitrequest
{
    public int id { get; set; }
    public int resident_id { get; set; }
    public string type { get; set; } = string.Empty; // "casual"|"especial"
    public string status { get; set; } = string.Empty; // "solicitado"|"en_proceso"|"autorizacion_tutor"|"autorizacion_preceptor"|"autorizado"|"rechazado"|"cancelado"
    public DateTime departure_at { get; set; }
    public DateTime return_eta_at { get; set; }
    public bool? authorized_tutor { get; set; }
    public bool? authorized_preceptor { get; set; }
    public DateTime? guard_exit_at { get; set; }
    public DateTime? guard_return_at { get; set; }
    
    // Navigation property
    public resident resident { get; set; } = null!;
}