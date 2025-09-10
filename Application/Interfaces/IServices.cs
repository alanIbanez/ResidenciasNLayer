namespace ResidenciasNLayer.Application.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(string expoToken, string title, string body);
    Task SendNotificationToRoleAsync(string role, string title, string body);
    Task SendNotificationToUsersAsync(IEnumerable<int> userIds, string title, string body);
}

public interface IAuthService
{
    Task<string> RegisterAsync(string email, string password, string firstname, string lastname, string role, string? expotoken);
    Task<string?> LoginAsync(string email, string password);
    Task UpdatePushTokenAsync(int userId, string expotoken);
    Task<string> GenerateJwtTokenAsync(int userId, string email, string role);
}

public interface IEventService
{
    Task<int> CreateEventAsync(string name, string description, DateTime dateAt, int createdBy);
    Task<IEnumerable<dynamic>> GetEventsAsync();
}