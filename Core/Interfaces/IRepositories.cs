using ResidenciasNLayer.Core.Entities;

namespace ResidenciasNLayer.Core.Interfaces;

public interface IUserRepository : IRepository<user>
{
    Task<user?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
}

public interface ITutorRepository : IRepository<tutor>
{
    Task<tutor?> GetByUserIdAsync(int userId);
}

public interface IResidentRepository : IRepository<resident>
{
    Task<IEnumerable<resident>> GetByTutorIdAsync(int tutorId);
    Task<resident?> GetByUserIdAsync(int userId);
}

public interface IExitRequestRepository : IRepository<exitrequest>
{
    Task<IEnumerable<exitrequest>> GetByResidentIdAsync(int residentId);
    Task<IEnumerable<exitrequest>> GetPendingRequestsAsync();
}

public interface IDeviceRepository : IRepository<device>
{
    Task<IEnumerable<device>> GetActiveByUserIdAsync(int userId);
    Task DeactivateAllByUserIdAsync(int userId);
}

public interface IUserTokenRepository : IRepository<usertoken>
{
    Task<usertoken?> GetByTokenAsync(string token);
    Task DeleteExpiredTokensAsync();
}