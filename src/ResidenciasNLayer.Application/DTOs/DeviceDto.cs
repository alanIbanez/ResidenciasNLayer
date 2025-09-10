namespace ResidenciasNLayer.Application.DTOs;

public class DeviceRegistrationDto
{
    public string deviceid { get; set; } = string.Empty;
    public string tokenfcm { get; set; } = string.Empty;
}

public class DeviceResponseDto
{
    public int id { get; set; }
    public string deviceid { get; set; } = string.Empty;
    public string tokenfcm { get; set; } = string.Empty;
    public int user_id { get; set; }
}
