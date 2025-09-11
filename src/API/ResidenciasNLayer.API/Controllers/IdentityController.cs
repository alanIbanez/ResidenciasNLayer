using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ResidenciasNLayer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IdentityController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult Me()
    {
        var user = HttpContext.User;
        var roles = user.FindAll("role").Select(c => c.Value).Distinct().ToArray();
        var claims = user.Claims.Select(c => new { type = c.Type, value = c.Value }).ToArray();

        return Ok(new
        {
            isAuthenticated = user.Identity?.IsAuthenticated ?? false,
            name = user.Identity?.Name,
            roles,
            claims
        });
    }
}