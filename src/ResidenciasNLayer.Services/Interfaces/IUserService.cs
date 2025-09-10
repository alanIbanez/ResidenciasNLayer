using ResidenciasNLayer.Entities;

namespace ResidenciasNLayer.Services.Interfaces;

/// <summary>
/// Interface for user service operations
/// </summary>
public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userid);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int userid);
    Task<bool> UserExistsAsync(int userid);
    Task<User?> GetUserByDeviceIdAsync(string deviceid);
}