using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly ResidenciasDbContext _context;

    public AttendanceRepository(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<Attendance> AddAsync(Attendance attendance)
    {
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
        return attendance;
    }

    public async Task<IEnumerable<Attendance>> AddRangeAsync(IEnumerable<Attendance> attendances)
    {
        _context.Attendances.AddRange(attendances);
        await _context.SaveChangesAsync();
        return attendances;
    }

    public async Task<IEnumerable<Attendance>> GetByResidentAsync(int residentId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.Attendances
            .Include(a => a.Resident)
                .ThenInclude(r => r.User)
            .Where(a => a.ResidentId == residentId);

        if (fromDate.HasValue)
            query = query.Where(a => a.OccurredAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(a => a.OccurredAt <= toDate.Value);

        return await query
            .OrderByDescending(a => a.OccurredAt)
            .ToListAsync();
    }
}