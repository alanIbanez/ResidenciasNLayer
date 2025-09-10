using Microsoft.Extensions.Configuration;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Core.Entities;
using ResidenciasNLayer.Core.Interfaces;
using System.Text;
using System.Text.Json;

namespace ResidenciasNLayer.Infrastructure.Services;

public class PushNotificationService : IPushNotificationService
{
    private readonly HttpClient _httpClient;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IConfiguration _configuration;
    private readonly string _expoPushUrl = "https://exp.host/--/api/v2/push/send";

    public PushNotificationService(HttpClient httpClient, IDeviceRepository deviceRepository, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _deviceRepository = deviceRepository;
        _configuration = configuration;
    }

    public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body)
    {
        try
        {
            var pushMessage = new
            {
                to = deviceToken,
                title = title,
                body = body,
                sound = "default"
            };

            var json = JsonSerializer.Serialize(pushMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_expoPushUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RegisterDeviceTokenAsync(int userId, string token)
    {
        try
        {
            // Deactivate existing tokens for the user
            await _deviceRepository.DeactivateAllByUserIdAsync(userId);

            // Add new device token
            var device = new device
            {
                user_id = userId,
                tokenfcm = token,
                active = true,
                created_at = DateTime.UtcNow
            };

            await _deviceRepository.AddAsync(device);
            return true;
        }
        catch
        {
            return false;
        }
    }
}