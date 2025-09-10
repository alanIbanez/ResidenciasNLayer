using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IExitRepository
{
    Task<Exit?> GetByIdAsync(int id);
    Task<Exit> AddAsync(Exit exit);
    Task<Exit> UpdateAsync(Exit exit);
    Task<IEnumerable<Exit>> ListAsync(int? residentId = null, int? exitStatusId = null, DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<ExitAuthorization>> GetAuthorizationsAsync(int exitId);
    Task<ExitAuthorization> AddAuthorizationAsync(ExitAuthorization authorization);
}