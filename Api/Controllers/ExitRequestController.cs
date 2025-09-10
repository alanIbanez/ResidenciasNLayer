using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;

namespace ResidenciasNLayer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExitRequestController : ControllerBase
{
    private readonly IExitRequestService _exitRequestService;

    public ExitRequestController(IExitRequestService exitRequestService)
    {
        _exitRequestService = exitRequestService;
    }

    [HttpGet("resident/{residentId}")]
    public async Task<ActionResult<IEnumerable<ExitRequestDto>>> GetExitRequestsByResident(int residentId)
    {
        var requests = await _exitRequestService.GetExitRequestsByResidentAsync(residentId);
        return Ok(requests);
    }

    [HttpPost("resident/{residentId}")]
    public async Task<ActionResult<ExitRequestDto>> CreateExitRequest(int residentId, [FromBody] CreateExitRequestDto request)
    {
        var exitRequest = await _exitRequestService.CreateExitRequestAsync(residentId, request);
        if (exitRequest == null)
        {
            return BadRequest("Failed to create exit request");
        }

        return CreatedAtAction(nameof(GetExitRequestsByResident), new { residentId }, exitRequest);
    }

    [HttpPut("{requestId}/authorize")]
    public async Task<ActionResult> AuthorizeExitRequest(int requestId, [FromBody] AuthorizeRequestDto request)
    {
        var result = await _exitRequestService.AuthorizeExitRequestAsync(requestId, request.isApproved, request.isTutor);
        if (!result)
        {
            return BadRequest("Failed to authorize exit request");
        }

        return Ok("Exit request authorization updated");
    }

    [HttpPut("{requestId}/exit")]
    public async Task<ActionResult> ProcessExit(int requestId)
    {
        var result = await _exitRequestService.ProcessExitAsync(requestId);
        if (!result)
        {
            return BadRequest("Failed to process exit");
        }

        return Ok("Exit processed successfully");
    }

    [HttpPut("{requestId}/return")]
    public async Task<ActionResult> ProcessReturn(int requestId)
    {
        var result = await _exitRequestService.ProcessReturnAsync(requestId);
        if (!result)
        {
            return BadRequest("Failed to process return");
        }

        return Ok("Return processed successfully");
    }
}

public class AuthorizeRequestDto
{
    public bool isApproved { get; set; }
    public bool isTutor { get; set; }
}