using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;

namespace ResidenciasNLayer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPushNotificationService _pushNotificationService;

    public AuthController(IAuthService authService, IPushNotificationService pushNotificationService)
    {
        _authService = authService;
        _pushNotificationService = pushNotificationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (result == null)
        {
            return BadRequest("Invalid username or password");
        }

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        if (result == null)
        {
            return BadRequest("Username already exists or registration failed");
        }

        return Ok(result);
    }

    [HttpPost("device-token")]
    public async Task<ActionResult> RegisterDeviceToken([FromBody] DeviceTokenDto request)
    {
        // Get user ID from JWT token (you'd implement this based on your JWT authentication)
        // For now, we'll assume it's passed or extracted from the authenticated user
        var userId = 1; // This should be extracted from the JWT token

        var result = await _pushNotificationService.RegisterDeviceTokenAsync(userId, request.tokenfcm);
        if (!result)
        {
            return BadRequest("Failed to register device token");
        }

        return Ok("Device token registered successfully");
    }
}