using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Core.Entities;
using ResidenciasNLayer.Core.Interfaces;

namespace ResidenciasNLayer.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IUserTokenRepository userTokenRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _userTokenRepository = userTokenRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.username);
        if (user == null || !_passwordService.VerifyPassword(request.password, user.passwordhash))
        {
            return null;
        }

        var token = _tokenService.GenerateJwtToken(user.username, user.role);
        
        // Store token in database
        await _userTokenRepository.AddAsync(new usertoken
        {
            user_id = user.id,
            token = token,
            issued_at = DateTime.UtcNow
        });

        return new LoginResponseDto
        {
            token = token,
            username = user.username,
            role = user.role
        };
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        if (await _userRepository.ExistsAsync(request.username))
        {
            return null;
        }

        var hashedPassword = _passwordService.HashPassword(request.password);
        var user = await _userRepository.AddAsync(new user
        {
            username = request.username,
            passwordhash = hashedPassword,
            role = request.role
        });

        var token = _tokenService.GenerateJwtToken(user.username, user.role);
        
        // Store token in database
        await _userTokenRepository.AddAsync(new usertoken
        {
            user_id = user.id,
            token = token,
            issued_at = DateTime.UtcNow
        });

        return new LoginResponseDto
        {
            token = token,
            username = user.username,
            role = user.role
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        if (!_tokenService.ValidateJwtToken(token))
        {
            return false;
        }

        var userToken = await _userTokenRepository.GetByTokenAsync(token);
        return userToken != null;
    }
}