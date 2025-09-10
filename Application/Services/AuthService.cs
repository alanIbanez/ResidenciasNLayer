using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResidenciasNLayer.Application.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(string email, string password, string firstname, string lastname, string role, string? expotoken)
    {
        // Check if user already exists
        var existingUser = await _context.users.FirstOrDefaultAsync(u => u.email == email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists");
        }

        // Hash password (in production, use proper password hashing)
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            email = email,
            password = hashedPassword,
            firstname = firstname,
            lastname = lastname,
            role = role,
            expotoken = expotoken,
            createdat = DateTime.UtcNow,
            updatedat = DateTime.UtcNow
        };

        _context.users.Add(user);
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = await GenerateJwtTokenAsync(user.id, user.email, user.role);

        // Store token in database
        var userToken = new UserToken
        {
            userid = user.id,
            token = token,
            expiry = DateTime.UtcNow.AddDays(30),
            createdat = DateTime.UtcNow
        };

        _context.usertokens.Add(userToken);
        await _context.SaveChangesAsync();

        return token;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.password))
        {
            return null;
        }

        // Generate JWT token
        var token = await GenerateJwtTokenAsync(user.id, user.email, user.role);

        // Store token in database
        var userToken = new UserToken
        {
            userid = user.id,
            token = token,
            expiry = DateTime.UtcNow.AddDays(30),
            createdat = DateTime.UtcNow
        };

        _context.usertokens.Add(userToken);
        await _context.SaveChangesAsync();

        return token;
    }

    public async Task UpdatePushTokenAsync(int userId, string expotoken)
    {
        var user = await _context.users.FindAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.expotoken = expotoken;
        user.updatedat = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<string> GenerateJwtTokenAsync(int userId, string email, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}