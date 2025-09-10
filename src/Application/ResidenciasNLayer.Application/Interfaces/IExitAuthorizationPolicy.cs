using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IExitAuthorizationPolicy
{
    Task<(bool tutorRequired, bool preceptorRequired)> GetRequiredAsync(Exit exit);
    Task<bool> IsFullyAuthorizedAsync(Exit exit);
    Task<bool> CanUserApproveAsync(int userId, Exit exit);
}