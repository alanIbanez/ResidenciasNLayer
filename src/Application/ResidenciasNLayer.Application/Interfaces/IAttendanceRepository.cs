using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface IAttendanceRepository
{
    Task<Attendance> AddAsync(Attendance attendance);
    Task<IEnumerable<Attendance>> AddRangeAsync(IEnumerable<Attendance> attendances);
    Task<IEnumerable<Attendance>> GetByResidentAsync(int residentId, DateTime? fromDate = null, DateTime? toDate = null);
}