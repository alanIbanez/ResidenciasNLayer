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

public interface IExitRequestService
{
    Task<int> CreateExitRequestAsync(int residentId, int? tutorId, string reason, DateTime requestDate);
    Task UpdateExitRequestStatusAsync(int requestId, string status, string approvedBy, int approverId);
    Task ProcessExitAsync(int requestId, int guardId);
    Task ProcessReturnAsync(int requestId, int guardId);
    Task<IEnumerable<dynamic>> GetExitRequestsAsync();
}