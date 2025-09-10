using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Core.Entities;
using ResidenciasNLayer.Core.Interfaces;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Repositories;

public class UserRepository : Repository<user>, IUserRepository
{
    public UserRepository(ResidenciasDbContext context) : base(context)
    {
    }

    public async Task<user?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.username == username);
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.username == username);
    }
}

public class TutorRepository : Repository<tutor>, ITutorRepository
{
    public TutorRepository(ResidenciasDbContext context) : base(context)
    {
    }

    public async Task<tutor?> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Include(t => t.user).FirstOrDefaultAsync(t => t.user_id == userId);
    }
}

public class ResidentRepository : Repository<resident>, IResidentRepository
{
    public ResidentRepository(ResidenciasDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<resident>> GetByTutorIdAsync(int tutorId)
    {
        return await _dbSet
            .Include(r => r.user)
            .Include(r => r.residenttype)
            .Where(r => r.tutor_id == tutorId)
            .ToListAsync();
    }

    public async Task<resident?> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Include(r => r.user)
            .Include(r => r.residenttype)
            .Include(r => r.tutor)
            .FirstOrDefaultAsync(r => r.user_id == userId);
    }
}

public class ExitRequestRepository : Repository<exitrequest>, IExitRequestRepository
{
    public ExitRequestRepository(ResidenciasDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<exitrequest>> GetByResidentIdAsync(int residentId)
    {
        return await _dbSet
            .Include(er => er.resident)
            .ThenInclude(r => r.user)
            .Where(er => er.resident_id == residentId)
            .OrderByDescending(er => er.departure_at)
            .ToListAsync();
    }

    public async Task<IEnumerable<exitrequest>> GetPendingRequestsAsync()
    {
        return await _dbSet
            .Include(er => er.resident)
            .ThenInclude(r => r.user)
            .Where(er => er.status == "solicitado" || er.status == "en_proceso" || 
                        er.status == "autorizacion_tutor" || er.status == "autorizacion_preceptor")
            .OrderBy(er => er.departure_at)
            .ToListAsync();
    }
}

public class DeviceRepository : Repository<device>, IDeviceRepository
{
    public DeviceRepository(ResidenciasDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<device>> GetActiveByUserIdAsync(int userId)
    {
        return await _dbSet.Where(d => d.user_id == userId && d.active).ToListAsync();
    }

    public async Task DeactivateAllByUserIdAsync(int userId)
    {
        var devices = await _dbSet.Where(d => d.user_id == userId).ToListAsync();
        foreach (var device in devices)
        {
            device.active = false;
        }
        await _context.SaveChangesAsync();
    }
}

public class UserTokenRepository : Repository<usertoken>, IUserTokenRepository
{
    public UserTokenRepository(ResidenciasDbContext context) : base(context)
    {
    }

    public async Task<usertoken?> GetByTokenAsync(string token)
    {
        return await _dbSet.Include(ut => ut.user).FirstOrDefaultAsync(ut => ut.token == token);
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var expiredTokens = await _dbSet
            .Where(ut => ut.issued_at < DateTime.UtcNow.AddDays(-30))
            .ToListAsync();
        
        _dbSet.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}