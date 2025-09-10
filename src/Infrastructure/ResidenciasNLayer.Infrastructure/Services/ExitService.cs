using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Constants;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Services;

public class ExitService : IExitService
{
    private readonly ResidenciasDbContext _context;
    private readonly IExitRepository _exitRepository;
    private readonly IUserRepository _userRepository;
    private readonly IExitAuthorizationPolicy _authorizationPolicy;
    private readonly INotificationService _notificationService;

    public ExitService(
        ResidenciasDbContext context,
        IExitRepository exitRepository,
        IUserRepository userRepository,
        IExitAuthorizationPolicy authorizationPolicy,
        INotificationService notificationService)
    {
        _context = context;
        _exitRepository = exitRepository;
        _userRepository = userRepository;
        _authorizationPolicy = authorizationPolicy;
        _notificationService = notificationService;
    }

    public async Task<Exit> RequestAsync(int residentUserId, int exitTypeId, DateTime plannedDepartureAt, DateTime plannedReturnAt, string? notes = null)
    {
        var resident = await _userRepository.GetResidentByUserIdAsync(residentUserId);
        if (resident == null)
            throw new InvalidOperationException("Resident not found");

        var solicitadoStatus = await _context.ExitStatuses
            .FirstOrDefaultAsync(es => es.Name == ExitStatusNames.Solicitado);
        
        if (solicitadoStatus == null)
            throw new InvalidOperationException("Exit status 'solicitado' not found");

        var exit = new Exit
        {
            ResidentId = resident.Id,
            ExitTypeId = exitTypeId,
            ExitStatusId = solicitadoStatus.Id,
            PlannedDepartureAt = plannedDepartureAt,
            PlannedReturnAt = plannedReturnAt,
            Notes = notes,
            RequestedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAsync(exit);

        // Record the request authorization
        var authorization = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = residentUserId,
            PerformedByRole = RoleNames.Residente,
            Action = ExitAuthorizationActions.Requested,
            PerformedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAuthorizationAsync(authorization);

        // Update status to "en_proceso"
        var enProcesoStatus = await _context.ExitStatuses
            .FirstOrDefaultAsync(es => es.Name == ExitStatusNames.EnProceso);
        
        if (enProcesoStatus != null)
        {
            exit.ExitStatusId = enProcesoStatus.Id;
            await _exitRepository.UpdateAsync(exit);
        }

        // Send notifications
        await _notificationService.NotifyExitRequestedAsync(exit);

        return exit;
    }

    public async Task<Exit> ApproveAsync(int exitId, int approverUserId)
    {
        var exit = await _exitRepository.GetByIdAsync(exitId);
        if (exit == null)
            throw new InvalidOperationException("Exit not found");

        var user = await _userRepository.GetByIdAsync(approverUserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!await _authorizationPolicy.CanUserApproveAsync(approverUserId, exit))
            throw new UnauthorizedAccessException("User cannot approve this exit");

        // Record the approval authorization
        var authorization = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = approverUserId,
            PerformedByRole = user.Role.Name,
            Action = ExitAuthorizationActions.Approved,
            PerformedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAuthorizationAsync(authorization);

        // Check if fully authorized
        if (await _authorizationPolicy.IsFullyAuthorizedAsync(exit))
        {
            var autorizadoStatus = await _context.ExitStatuses
                .FirstOrDefaultAsync(es => es.Name == ExitStatusNames.Autorizado);
            
            if (autorizadoStatus != null)
            {
                exit.ExitStatusId = autorizadoStatus.Id;
                await _exitRepository.UpdateAsync(exit);
            }
        }

        await _notificationService.NotifyExitApprovedAsync(exit, user);

        return exit;
    }

    public async Task<Exit> RejectAsync(int exitId, int approverUserId, string reason)
    {
        var exit = await _exitRepository.GetByIdAsync(exitId);
        if (exit == null)
            throw new InvalidOperationException("Exit not found");

        var user = await _userRepository.GetByIdAsync(approverUserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!await _authorizationPolicy.CanUserApproveAsync(approverUserId, exit))
            throw new UnauthorizedAccessException("User cannot reject this exit");

        // Record the rejection authorization
        var authorization = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = approverUserId,
            PerformedByRole = user.Role.Name,
            Action = ExitAuthorizationActions.Rejected,
            Reason = reason,
            PerformedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAuthorizationAsync(authorization);

        // Update status to rejected
        var rechazadoStatus = await _context.ExitStatuses
            .FirstOrDefaultAsync(es => es.Name == ExitStatusNames.Rechazado);
        
        if (rechazadoStatus != null)
        {
            exit.ExitStatusId = rechazadoStatus.Id;
            await _exitRepository.UpdateAsync(exit);
        }

        await _notificationService.NotifyExitRejectedAsync(exit, user, reason);

        return exit;
    }

    public async Task<Exit> CancelAsync(int exitId, int byUserId, string? reason = null)
    {
        var exit = await _exitRepository.GetByIdAsync(exitId);
        if (exit == null)
            throw new InvalidOperationException("Exit not found");

        var user = await _userRepository.GetByIdAsync(byUserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        // Record the cancellation authorization
        var authorization = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = byUserId,
            PerformedByRole = user.Role.Name,
            Action = ExitAuthorizationActions.Canceled,
            Reason = reason,
            PerformedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAuthorizationAsync(authorization);

        // Update status to canceled
        var canceladoStatus = await _context.ExitStatuses
            .FirstOrDefaultAsync(es => es.Name == ExitStatusNames.Cancelado);
        
        if (canceladoStatus != null)
        {
            exit.ExitStatusId = canceladoStatus.Id;
            await _exitRepository.UpdateAsync(exit);
        }

        await _notificationService.NotifyExitCanceledAsync(exit, user, reason);

        return exit;
    }

    public async Task<Exit> RecordGuardDepartureAsync(int exitId, int guardUserId, DateTime at)
    {
        var exit = await _exitRepository.GetByIdAsync(exitId);
        if (exit == null)
            throw new InvalidOperationException("Exit not found");

        // Verify exit is authorized
        if (!await _authorizationPolicy.IsFullyAuthorizedAsync(exit))
            throw new InvalidOperationException("Exit is not fully authorized");

        var guard = await _userRepository.GetGuardByUserIdAsync(guardUserId);
        if (guard == null)
            throw new InvalidOperationException("Guard not found");

        // Record actual departure time
        exit.ActualDepartureAt = at;
        await _exitRepository.UpdateAsync(exit);

        // Record the guard action
        var authorization = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = guardUserId,
            PerformedByRole = RoleNames.Guardia,
            Action = ExitAuthorizationActions.GuardDeparture,
            PerformedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAuthorizationAsync(authorization);

        await _notificationService.NotifyGuardDepartureAsync(exit, guard.User);

        return exit;
    }

    public async Task<Exit> RecordGuardReturnAsync(int exitId, int guardUserId, DateTime at)
    {
        var exit = await _exitRepository.GetByIdAsync(exitId);
        if (exit == null)
            throw new InvalidOperationException("Exit not found");

        var guard = await _userRepository.GetGuardByUserIdAsync(guardUserId);
        if (guard == null)
            throw new InvalidOperationException("Guard not found");

        // Record actual return time
        exit.ActualReturnAt = at;
        await _exitRepository.UpdateAsync(exit);

        // Record the guard action
        var authorization = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = guardUserId,
            PerformedByRole = RoleNames.Guardia,
            Action = ExitAuthorizationActions.GuardReturn,
            PerformedAt = DateTime.UtcNow
        };

        await _exitRepository.AddAuthorizationAsync(authorization);

        await _notificationService.NotifyGuardReturnAsync(exit, guard.User);

        return exit;
    }

    public async Task<Exit?> GetByIdAsync(int exitId)
    {
        return await _exitRepository.GetByIdAsync(exitId);
    }

    public async Task<IEnumerable<Exit>> ListAsync(int? residentId = null, int? exitStatusId = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        return await _exitRepository.ListAsync(residentId, exitStatusId, fromDate, toDate);
    }
}