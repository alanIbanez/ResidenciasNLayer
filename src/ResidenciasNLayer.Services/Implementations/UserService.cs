using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Data;
using ResidenciasNLayer.Entities;
using ResidenciasNLayer.Services.Interfaces;

namespace ResidenciasNLayer.Services.Implementations;

/// <summary>
/// Implementation of user service operations
/// </summary>
public class UserService : IUserService
{
    private readonly ResidenciasDbContext _context;

    public UserService(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int userid)
    {
        return await _context.Users
            .Include(u => u.Device)
            .FirstOrDefaultAsync(u => u.userid == userid);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Device)
            .ToListAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.userid);
        if (existingUser == null)
        {
            return null;
        }

        existingUser.username = user.username;
        existingUser.email = user.email;
        existingUser.deviceid = user.deviceid;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int userid)
    {
        var user = await _context.Users.FindAsync(userid);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UserExistsAsync(int userid)
    {
        return await _context.Users.AnyAsync(u => u.userid == userid);
    }

    public async Task<User?> GetUserByDeviceIdAsync(string deviceid)
    {
        return await _context.Users
            .Include(u => u.Device)
            .FirstOrDefaultAsync(u => u.deviceid == deviceid);
    }
}