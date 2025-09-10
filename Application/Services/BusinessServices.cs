using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Core.Interfaces;

namespace ResidenciasNLayer.Application.Services;

public class ResidentService : IResidentService
{
    private readonly IResidentRepository _residentRepository;

    public ResidentService(IResidentRepository residentRepository)
    {
        _residentRepository = residentRepository;
    }

    public async Task<IEnumerable<UserDto>> GetResidentsByTutorAsync(int tutorId)
    {
        var residents = await _residentRepository.GetByTutorIdAsync(tutorId);
        return residents.Select(r => new UserDto
        {
            id = r.user.id,
            username = r.user.username,
            role = r.user.role
        });
    }
}

public class AttendanceService : IAttendanceService
{
    private readonly IRepository<ResidenciasNLayer.Core.Entities.attendance> _attendanceRepository;

    public AttendanceService(IRepository<ResidenciasNLayer.Core.Entities.attendance> attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public async Task<bool> MarkAttendanceAsync(int residentId, string type, int? eventId = null)
    {
        try
        {
            var attendance = new ResidenciasNLayer.Core.Entities.attendance
            {
                resident_id = residentId,
                type = type,
                date_at = DateTime.UtcNow,
                event_id = eventId
            };

            await _attendanceRepository.AddAsync(attendance);
            return true;
        }
        catch
        {
            return false;
        }
    }
}