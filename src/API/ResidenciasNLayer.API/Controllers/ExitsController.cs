using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExitsController : ControllerBase
{
    private readonly IExitService _exitService;
    private readonly IExitAuthorizationPolicy _authorizationPolicy;

    public ExitsController(IExitService exitService, IExitAuthorizationPolicy authorizationPolicy)
    {
        _exitService = exitService;
        _authorizationPolicy = authorizationPolicy;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Exit>>> GetExits(
        [FromQuery] int? residentId = null,
        [FromQuery] int? exitStatusId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var exits = await _exitService.ListAsync(residentId, exitStatusId, fromDate, toDate);
        return Ok(exits);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Exit>> GetExit(int id)
    {
        var exit = await _exitService.GetByIdAsync(id);
        if (exit == null)
            return NotFound();

        return Ok(exit);
    }

    [HttpPost]
    [Authorize(Policy = "ResidenteOnly")]
    public async Task<ActionResult<Exit>> RequestExit([FromBody] ExitRequestDto request)
    {
        try
        {
            var exit = await _exitService.RequestAsync(
                request.ResidentUserId,
                request.ExitTypeId,
                request.PlannedDepartureAt,
                request.PlannedReturnAt,
                request.Notes);

            return CreatedAtAction(nameof(GetExit), new { id = exit.Id }, exit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = "PreceptorOrTutor")]
    public async Task<ActionResult<Exit>> ApproveExit(int id, [FromBody] ApprovalDto approval)
    {
        try
        {
            var exit = await _exitService.ApproveAsync(id, approval.ApproverUserId);
            return Ok(exit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/reject")]
    [Authorize(Policy = "PreceptorOrTutor")]
    public async Task<ActionResult<Exit>> RejectExit(int id, [FromBody] RejectionDto rejection)
    {
        try
        {
            var exit = await _exitService.RejectAsync(id, rejection.ApproverUserId, rejection.Reason);
            return Ok(exit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/cancel")]
    [Authorize]
    public async Task<ActionResult<Exit>> CancelExit(int id, [FromBody] CancellationDto cancellation)
    {
        try
        {
            var exit = await _exitService.CancelAsync(id, cancellation.UserId, cancellation.Reason);
            return Ok(exit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/guard/departure")]
    [Authorize(Policy = "GuardiaOnly")]
    public async Task<ActionResult<Exit>> RecordDeparture(int id, [FromBody] GuardActionDto action)
    {
        try
        {
            var exit = await _exitService.RecordGuardDepartureAsync(id, action.GuardUserId, action.At);
            return Ok(exit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/guard/return")]
    [Authorize(Policy = "GuardiaOnly")]
    public async Task<ActionResult<Exit>> RecordReturn(int id, [FromBody] GuardActionDto action)
    {
        try
        {
            var exit = await _exitService.RecordGuardReturnAsync(id, action.GuardUserId, action.At);
            return Ok(exit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class ExitRequestDto
{
    public int ResidentUserId { get; set; }
    public int ExitTypeId { get; set; }
    public DateTime PlannedDepartureAt { get; set; }
    public DateTime PlannedReturnAt { get; set; }
    public string? Notes { get; set; }
}

public class ApprovalDto
{
    public int ApproverUserId { get; set; }
}

public class RejectionDto
{
    public int ApproverUserId { get; set; }
    public string Reason { get; set; } = null!;
}

public class CancellationDto
{
    public int UserId { get; set; }
    public string? Reason { get; set; }
}

public class GuardActionDto
{
    public int GuardUserId { get; set; }
    public DateTime At { get; set; }
}