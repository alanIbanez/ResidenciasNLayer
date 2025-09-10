using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface INotificationService
{
    Task NotifyExitRequestedAsync(Exit exit);
    Task NotifyExitApprovedAsync(Exit exit, User approver);
    Task NotifyExitRejectedAsync(Exit exit, User approver, string reason);
    Task NotifyExitCanceledAsync(Exit exit, User canceledBy, string? reason);
    Task NotifyGuardDepartureAsync(Exit exit, User guard);
    Task NotifyGuardReturnAsync(Exit exit, User guard);
    Task NotifyEventPublishedAsync(int eventId, IEnumerable<User> recipients);
}