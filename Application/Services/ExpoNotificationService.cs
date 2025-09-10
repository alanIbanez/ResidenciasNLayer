using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Infrastructure.Data;
using System.Text;
using System.Text.Json;

namespace ResidenciasNLayer.Application.Services;

public class ExpoNotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;
    private const string ExpoApiUrl = "https://exp.host/--/api/v2/push/send";

    public ExpoNotificationService(ApplicationDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task SendNotificationAsync(string expoToken, string title, string body)
    {
        if (string.IsNullOrEmpty(expoToken) || !expoToken.StartsWith("ExponentPushToken["))
        {
            return; // Invalid token format
        }

        var notification = new
        {
            to = expoToken,
            title = title,
            body = body,
            sound = "default"
        };

        try
        {
            var json = JsonSerializer.Serialize(notification);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            await _httpClient.PostAsync(ExpoApiUrl, content);
        }
        catch (Exception)
        {
            // Log error but don't throw - notifications failing shouldn't break the app
        }
    }

    public async Task SendNotificationToRoleAsync(string role, string title, string body)
    {
        var users = await _context.users
            .Where(u => u.role == role && !string.IsNullOrEmpty(u.expotoken))
            .Select(u => u.expotoken!)
            .ToListAsync();

        var tasks = users.Select(token => SendNotificationAsync(token, title, body));
        await Task.WhenAll(tasks);
    }

    public async Task SendNotificationToUsersAsync(IEnumerable<int> userIds, string title, string body)
    {
        var users = await _context.users
            .Where(u => userIds.Contains(u.id) && !string.IsNullOrEmpty(u.expotoken))
            .Select(u => u.expotoken!)
            .ToListAsync();

        var tasks = users.Select(token => SendNotificationAsync(token, title, body));
        await Task.WhenAll(tasks);
    }
}