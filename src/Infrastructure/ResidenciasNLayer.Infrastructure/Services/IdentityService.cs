using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly ResidenciasDbContext _context;
    private readonly JwtTokenService _jwtTokenService;

    public IdentityService(ResidenciasDbContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(bool Success, string? Token, User? User)> RegisterAsync(string username, string password, string fullName, string roleName, string? pushToken = null)
    {
        try
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return (false, null, null);
            }

            // Find the role
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                return (false, null, null);
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Create the user
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                FullName = fullName,
                RoleId = role.Id,
                PushToken = pushToken,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Load the role for token generation
            await _context.Entry(user).Reference(u => u.Role).LoadAsync();

            // Generate token
            var token = _jwtTokenService.GenerateAccessToken(user);

            return (true, token, user);
        }
        catch
        {
            return (false, null, null);
        }
    }

    public async Task<(bool Success, string? Token, User? User)> ValidateCredentialsAsync(string username, string password)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
            {
                return (false, null, null);
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return (false, null, null);
            }

            // Generate token
            var token = _jwtTokenService.GenerateAccessToken(user);

            return (true, token, user);
        }
        catch
        {
            return (false, null, null);
        }
    }

    public string CreateAccessToken(User user)
    {
        return _jwtTokenService.GenerateAccessToken(user);
    }
}