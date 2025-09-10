using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Infrastructure.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUserRepository _userRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository, IUserRepository userRepository)
    {
        _attendanceRepository = attendanceRepository;
        _userRepository = userRepository;
    }

    public async Task<Attendance> RegisterAsync(int residentUserId, string? type, DateTime? occurredAt = null, int? eventId = null)
    {
        var resident = await _userRepository.GetResidentByUserIdAsync(residentUserId);
        if (resident == null)
            throw new InvalidOperationException("Resident not found");

        var attendance = new Attendance
        {
            ResidentId = resident.Id,
            Type = type,
            OccurredAt = occurredAt ?? DateTime.UtcNow,
            EventId = eventId,
            RegisteredAt = DateTime.UtcNow
        };

        return await _attendanceRepository.AddAsync(attendance);
    }

    public async Task<IEnumerable<Attendance>> RegisterBulkAsync(IEnumerable<AttendanceRegistrationDto> entries)
    {
        var attendances = new List<Attendance>();

        foreach (var entry in entries)
        {
            var resident = await _userRepository.GetResidentByUserIdAsync(entry.ResidentUserId);
            if (resident == null)
                continue; // Skip invalid residents

            var attendance = new Attendance
            {
                ResidentId = resident.Id,
                Type = entry.Type,
                OccurredAt = entry.OccurredAt ?? DateTime.UtcNow,
                EventId = entry.EventId,
                RegisteredAt = DateTime.UtcNow
            };

            attendances.Add(attendance);
        }

        return await _attendanceRepository.AddRangeAsync(attendances);
    }

    public async Task<IEnumerable<Attendance>> GetByResidentAsync(int residentId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        return await _attendanceRepository.GetByResidentAsync(residentId, fromDate, toDate);
    }
}