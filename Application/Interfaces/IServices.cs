using ResidenciasNLayer.Application.DTOs;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto request);
    Task<bool> ValidateTokenAsync(string token);
}

public interface ITokenService
{
    string GenerateJwtToken(string username, string role);
    bool ValidateJwtToken(string token);
    string? GetUsernameFromToken(string token);
}

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public interface IPushNotificationService
{
    Task<bool> SendNotificationAsync(string deviceToken, string title, string body);
    Task<bool> RegisterDeviceTokenAsync(int userId, string token);
}