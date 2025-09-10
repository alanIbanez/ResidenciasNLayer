using Microsoft.Extensions.Logging;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace ResidenciasNLayer.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationService(
        INotificationRepository notificationRepository, 
        ILogger<NotificationService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
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
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification, exit.Resident.User.PushToken);
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
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification, exit.Resident.User.PushToken);
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
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification, exit.Resident.User.PushToken);
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
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification, exit.Resident.User.PushToken);
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
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification, exit.Resident.User.PushToken);
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
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await SendPushNotificationAsync(notification, exit.Resident.User.PushToken);
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
                Status = "pending",
                SentAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await SendPushNotificationAsync(notification, recipient.PushToken);
        }
    }

    private async Task SendPushNotificationAsync(Notification notification, string? pushToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(pushToken))
            {
                _logger.LogWarning($"No push token available for user {notification.UserId}");
                notification.Status = "failed";
                notification.ProviderResponse = "No push token available";
                await _notificationRepository.UpdateAsync(notification);
                return;
            }

            var httpClient = _httpClientFactory.CreateClient("ExpoNotifications");

            var expoPushMessage = new
            {
                to = pushToken,
                title = notification.Title,
                body = notification.Body,
                data = new
                {
                    exitId = notification.ExitId,
                    eventId = notification.EventId,
                    type = notification.Type
                }
            };

            var json = JsonSerializer.Serialize(expoPushMessage, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync("--/api/v2/push/send", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Successfully sent push notification to user {notification.UserId}");
                
                // Try to extract the receipt ID from response
                var responseData = JsonSerializer.Deserialize<JsonElement>(responseContent);
                if (responseData.TryGetProperty("data", out var dataArray) && dataArray.GetArrayLength() > 0)
                {
                    var firstItem = dataArray[0];
                    if (firstItem.TryGetProperty("id", out var idProperty))
                    {
                        notification.ProviderMessageId = idProperty.GetString();
                    }
                }

                notification.Status = "sent";
                notification.ProviderResponse = responseContent;
            }
            else
            {
                _logger.LogError($"Failed to send push notification to user {notification.UserId}. Status: {response.StatusCode}, Response: {responseContent}");
                notification.Status = "failed";
                notification.ProviderResponse = responseContent;
            }

            await _notificationRepository.UpdateAsync(notification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception occurred while sending push notification to user {notification.UserId}");
            notification.Status = "failed";
            notification.ProviderResponse = ex.Message;
            await _notificationRepository.UpdateAsync(notification);
        }
    }
}