using ResidenciasNLayer.Application.DTOs;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IExitRequestService
{
    Task<IEnumerable<ExitRequestDto>> GetExitRequestsByResidentAsync(int residentId);
    Task<ExitRequestDto?> CreateExitRequestAsync(int residentId, CreateExitRequestDto request);
    Task<bool> AuthorizeExitRequestAsync(int requestId, bool isApproved, bool isTutor);
    Task<bool> ProcessExitAsync(int requestId);
    Task<bool> ProcessReturnAsync(int requestId);
}

public interface IResidentService
{
    Task<IEnumerable<UserDto>> GetResidentsByTutorAsync(int tutorId);
}

public interface IAttendanceService
{
    Task<bool> MarkAttendanceAsync(int residentId, string type, int? eventId = null);
}