using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string? Token, User? User)> RegisterAsync(string username, string password, string fullName, string roleName, string? pushToken = null);
    Task<(bool Success, string? Token, User? User)> ValidateCredentialsAsync(string username, string password);
    string CreateAccessToken(User user);
}