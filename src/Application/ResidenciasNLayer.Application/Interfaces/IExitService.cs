using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IExitService
{
    Task<Exit> RequestAsync(int residentUserId, int exitTypeId, DateTime plannedDepartureAt, DateTime plannedReturnAt, string? notes = null);
    Task<Exit> ApproveAsync(int exitId, int approverUserId);
    Task<Exit> RejectAsync(int exitId, int approverUserId, string reason);
    Task<Exit> CancelAsync(int exitId, int byUserId, string? reason = null);
    Task<Exit> RecordGuardDepartureAsync(int exitId, int guardUserId, DateTime at);
    Task<Exit> RecordGuardReturnAsync(int exitId, int guardUserId, DateTime at);
    Task<Exit?> GetByIdAsync(int exitId);
    Task<IEnumerable<Exit>> ListAsync(int? residentId = null, int? exitStatusId = null, DateTime? fromDate = null, DateTime? toDate = null);
}