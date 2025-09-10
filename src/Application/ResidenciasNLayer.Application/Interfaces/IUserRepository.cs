using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
    Task<Resident?> GetResidentByUserIdAsync(int userId);
    Task<Preceptor?> GetPreceptorByUserIdAsync(int userId);
    Task<Tutor?> GetTutorByUserIdAsync(int userId);
    Task<Guard?> GetGuardByUserIdAsync(int userId);
}