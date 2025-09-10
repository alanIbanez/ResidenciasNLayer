using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;
using System.Security.Claims;

namespace ResidenciasNLayer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExitRequestsController : ControllerBase
{
    private readonly IExitRequestService _exitRequestService;

    public ExitRequestsController(IExitRequestService exitRequestService)
    {
        _exitRequestService = exitRequestService;
    }

    [HttpPost]
    [Authorize(Roles = "residente")]
    public async Task<IActionResult> CreateExitRequest([FromBody] CreateExitRequestDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int residentId))
            {
                return Unauthorized();
            }

            var requestId = await _exitRequestService.CreateExitRequestAsync(
                residentId,
                request.tutorid,
                request.reason,
                request.requestdate
            );

            return Ok(new { id = requestId, message = "Exit request created successfully" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "preceptor,tutor")]
    public async Task<IActionResult> UpdateExitRequestStatus(int id, [FromBody] UpdateExitRequestDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int approverId) || roleClaim == null)
            {
                return Unauthorized();
            }

            await _exitRequestService.UpdateExitRequestStatusAsync(id, request.status, roleClaim.Value, approverId);
            return Ok(new { message = "Exit request status updated successfully" });
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

    [HttpPut("{id}/exit")]
    [Authorize(Roles = "guardia")]
    public async Task<IActionResult> ProcessExit(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int guardId))
            {
                return Unauthorized();
            }

            await _exitRequestService.ProcessExitAsync(id, guardId);
            return Ok(new { message = "Exit processed successfully" });
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

    [HttpPut("{id}/return")]
    [Authorize(Roles = "guardia")]
    public async Task<IActionResult> ProcessReturn(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int guardId))
            {
                return Unauthorized();
            }

            await _exitRequestService.ProcessReturnAsync(id, guardId);
            return Ok(new { message = "Return processed successfully" });
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

    [HttpGet]
    [Authorize(Roles = "preceptor,guardia,tutor")]
    public async Task<IActionResult> GetExitRequests()
    {
        try
        {
            var exitRequests = await _exitRequestService.GetExitRequestsAsync();
            return Ok(exitRequests);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}