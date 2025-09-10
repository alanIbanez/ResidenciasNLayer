namespace ResidenciasNLayer.Application.DTOs;

public class RegisterRequestDto
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public string? expotoken { get; set; }
}

public class LoginRequestDto
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string token { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
}

public class UpdatePushTokenDto
{
    public string expotoken { get; set; } = string.Empty;
}

public class CreateEventDto
{
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public DateTime date_at { get; set; }
}

public class EventResponseDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public DateTime date_at { get; set; }
    public string created_by_name { get; set; } = string.Empty;
    public DateTime createdat { get; set; }
}

public class CreateExitRequestDto
{
    public int? tutorid { get; set; }
    public string reason { get; set; } = string.Empty;
    public DateTime requestdate { get; set; }
}

public class UpdateExitRequestDto
{
    public string status { get; set; } = string.Empty; // approved, rejected
}