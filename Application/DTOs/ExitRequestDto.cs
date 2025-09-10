namespace ResidenciasNLayer.Application.DTOs;

public class ExitRequestDto
{
    public int id { get; set; }
    public int resident_id { get; set; }
    public string type { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
    public DateTime departure_at { get; set; }
    public DateTime return_eta_at { get; set; }
    public bool? authorized_tutor { get; set; }
    public bool? authorized_preceptor { get; set; }
    public DateTime? guard_exit_at { get; set; }
    public DateTime? guard_return_at { get; set; }
}

public class CreateExitRequestDto
{
    public string type { get; set; } = string.Empty;
    public DateTime departure_at { get; set; }
    public DateTime return_eta_at { get; set; }
}

public class DeviceTokenDto
{
    public string tokenfcm { get; set; } = string.Empty;
}