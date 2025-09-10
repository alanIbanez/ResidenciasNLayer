using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IAttendanceService
{
    Task<Attendance> RegisterAsync(int residentUserId, string? type, DateTime? occurredAt = null, int? eventId = null);
    Task<IEnumerable<Attendance>> RegisterBulkAsync(IEnumerable<AttendanceRegistrationDto> entries);
    Task<IEnumerable<Attendance>> GetByResidentAsync(int residentId, DateTime? fromDate = null, DateTime? toDate = null);
}

public class AttendanceRegistrationDto
{
    public int ResidentUserId { get; set; }
    public string? Type { get; set; }
    public DateTime? OccurredAt { get; set; }
    public int? EventId { get; set; }
}