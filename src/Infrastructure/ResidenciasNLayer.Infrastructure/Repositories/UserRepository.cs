using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ResidenciasDbContext _context;

    public UserRepository(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.IsActive)
            .ToListAsync();
    }

    public async Task<Resident?> GetResidentByUserIdAsync(int userId)
    {
        return await _context.Residents
            .Include(r => r.User)
            .Include(r => r.ResidentType)
            .Include(r => r.Tutor)
            .FirstOrDefaultAsync(r => r.UserId == userId);
    }

    public async Task<Preceptor?> GetPreceptorByUserIdAsync(int userId)
    {
        return await _context.Preceptors
            .Include(p => p.User)
            .Include(p => p.PreceptorType)
            .Include(p => p.Shift)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Tutor?> GetTutorByUserIdAsync(int userId)
    {
        return await _context.Tutors
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<Guard?> GetGuardByUserIdAsync(int userId)
    {
        return await _context.Guards
            .Include(g => g.User)
            .Include(g => g.Shift)
            .FirstOrDefaultAsync(g => g.UserId == userId);
    }
}