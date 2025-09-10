using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Repositories;

public class ExitRepository : IExitRepository
{
    private readonly ResidenciasDbContext _context;

    public ExitRepository(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<Exit?> GetByIdAsync(int id)
    {
        return await _context.Exits
            .Include(e => e.Resident)
                .ThenInclude(r => r.User)
            .Include(e => e.Resident)
                .ThenInclude(r => r.ResidentType)
            .Include(e => e.ExitType)
            .Include(e => e.ExitStatus)
            .Include(e => e.Authorizations)
                .ThenInclude(a => a.PerformedByUser)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Exit> AddAsync(Exit exit)
    {
        _context.Exits.Add(exit);
        await _context.SaveChangesAsync();
        return exit;
    }

    public async Task<Exit> UpdateAsync(Exit exit)
    {
        _context.Exits.Update(exit);
        await _context.SaveChangesAsync();
        return exit;
    }

    public async Task<IEnumerable<Exit>> ListAsync(int? residentId = null, int? exitStatusId = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.Exits
            .Include(e => e.Resident)
                .ThenInclude(r => r.User)
            .Include(e => e.ExitType)
            .Include(e => e.ExitStatus)
            .AsQueryable();

        if (residentId.HasValue)
            query = query.Where(e => e.ResidentId == residentId.Value);

        if (exitStatusId.HasValue)
            query = query.Where(e => e.ExitStatusId == exitStatusId.Value);

        if (fromDate.HasValue)
            query = query.Where(e => e.RequestedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(e => e.RequestedAt <= toDate.Value);

        return await query.OrderByDescending(e => e.RequestedAt).ToListAsync();
    }

    public async Task<IEnumerable<ExitAuthorization>> GetAuthorizationsAsync(int exitId)
    {
        return await _context.ExitAuthorizations
            .Include(ea => ea.PerformedByUser)
            .Where(ea => ea.ExitId == exitId)
            .OrderBy(ea => ea.PerformedAt)
            .ToListAsync();
    }

    public async Task<ExitAuthorization> AddAuthorizationAsync(ExitAuthorization authorization)
    {
        _context.ExitAuthorizations.Add(authorization);
        await _context.SaveChangesAsync();
        return authorization;
    }
}