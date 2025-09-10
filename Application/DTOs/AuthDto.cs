namespace ResidenciasNLayer.Application.DTOs;

public class LoginRequestDto
{
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string token { get; set; } = string.Empty;
    public string username { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
}

public class RegisterRequestDto
{
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
}

public class UserDto
{
    public int id { get; set; }
    public string username { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
}