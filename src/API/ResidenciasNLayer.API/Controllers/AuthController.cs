using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.Interfaces;
using System.Security.Claims;

namespace ResidenciasNLayer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] RegisterDto request)
    {
        var result = await _identityService.RegisterAsync(
            request.Username,
            request.Password,
            request.FullName,
            request.RoleName,
            request.PushToken);

        if (!result.Success)
        {
            return BadRequest(new { message = "Registration failed. Username may already exist or role is invalid." });
        }

        return Ok(new
        {
            token = result.Token,
            user = new
            {
                id = result.User!.Id,
                username = result.User.Username,
                fullName = result.User.FullName,
                role = result.User.Role.Name,
                isActive = result.User.IsActive
            }
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginDto request)
    {
        var result = await _identityService.ValidateCredentialsAsync(request.Username, request.Password);

        if (!result.Success)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(new
        {
            token = result.Token,
            user = new
            {
                id = result.User!.Id,
                username = result.User.Username,
                fullName = result.User.FullName,
                role = result.User.Role.Name,
                isActive = result.User.IsActive
            }
        });
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var fullName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            id = userId,
            username = username,
            fullName = fullName,
            role = role,
            claims = User.Claims.Select(c => new { type = c.Type, value = c.Value })
        });
    }
}

public class RegisterDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public string? PushToken { get; set; }
}

public class LoginDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}