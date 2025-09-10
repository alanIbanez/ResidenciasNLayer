using Microsoft.Extensions.Logging;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task NotifyExitRequestedAsync(Exit exit)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_requested",
            Title = "Solicitud de Salida Enviada",
            Body = $"Tu solicitud de salida {exit.ExitType.Name} ha sido enviada para aprobación.",
            ExitId = exit.Id,
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification);
    }

    public async Task NotifyExitApprovedAsync(Exit exit, User approver)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_approved",
            Title = "Solicitud de Salida Aprobada",
            Body = $"Tu solicitud de salida ha sido aprobada por {approver.FullName}.",
            ExitId = exit.Id,
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification);
    }

    public async Task NotifyExitRejectedAsync(Exit exit, User approver, string reason)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_rejected",
            Title = "Solicitud de Salida Rechazada",
            Body = $"Tu solicitud de salida ha sido rechazada por {approver.FullName}. Motivo: {reason}",
            ExitId = exit.Id,
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification);
    }

    public async Task NotifyExitCanceledAsync(Exit exit, User canceledBy, string? reason)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_canceled",
            Title = "Solicitud de Salida Cancelada",
            Body = $"Tu solicitud de salida ha sido cancelada. {(reason != null ? $"Motivo: {reason}" : "")}",
            ExitId = exit.Id,
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification);
    }

    public async Task NotifyGuardDepartureAsync(Exit exit, User guard)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "guard_departure",
            Title = "Salida Registrada",
            Body = $"Tu salida ha sido registrada por el guardia {guard.FullName}.",
            ExitId = exit.Id,
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification);
    }

    public async Task NotifyGuardReturnAsync(Exit exit, User guard)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "guard_return",
            Title = "Regreso Registrado",
            Body = $"Tu regreso ha sido registrado por el guardia {guard.FullName}.",
            ExitId = exit.Id,
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification);
    }

    public async Task NotifyEventPublishedAsync(int eventId, IEnumerable<User> recipients)
    {
        foreach (var recipient in recipients)
        {
            var notification = new Notification
            {
                UserId = recipient.Id,
                Type = "event_published",
                Title = "Nuevo Evento Publicado",
                Body = "Se ha publicado un nuevo evento. Revisa los detalles en la aplicación.",
                EventId = eventId,
                SentAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await SendPushNotificationAsync(notification);
        }
    }

    private async Task SendPushNotificationAsync(Notification notification)
    {
        try
        {
            // Here you would implement the actual push notification logic
            // For example, using Firebase Cloud Messaging (FCM) or another service
            
            // For now, we'll just log it and mark as sent
            _logger.LogInformation($"Sending push notification to user {notification.UserId}: {notification.Title}");
            
            notification.Status = "sent";
            notification.ProviderMessageId = Guid.NewGuid().ToString();
            
            await _notificationRepository.UpdateAsync(notification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send push notification to user {notification.UserId}");
            notification.Status = "failed";
            await _notificationRepository.UpdateAsync(notification);
        }
    }
}