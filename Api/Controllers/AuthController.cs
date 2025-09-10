using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Infrastructure.Data;
using System.Security.Claims;

namespace ResidenciasNLayer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _context;

    public AuthController(IAuthService authService, ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var token = await _authService.RegisterAsync(
                request.email,
                request.password,
                request.firstname,
                request.lastname,
                request.role,
                request.expotoken
            );

            var user = await _context.users.FirstAsync(u => u.email == request.email);
            
            var response = new AuthResponseDto
            {
                token = token,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname,
                role = user.role
            };

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.email, request.password);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var user = await _context.users.FirstAsync(u => u.email == request.email);
            
            var response = new AuthResponseDto
            {
                token = token,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname,
                role = user.role
            };

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPut("push-token")]
    [Authorize]
    public async Task<IActionResult> UpdatePushToken([FromBody] UpdatePushTokenDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            await _authService.UpdatePushTokenAsync(userId, request.expotoken);
            return Ok(new { message = "Push token updated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}