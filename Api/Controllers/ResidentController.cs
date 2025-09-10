using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;

namespace ResidenciasNLayer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResidentController : ControllerBase
{
    private readonly IResidentService _residentService;

    public ResidentController(IResidentService residentService)
    {
        _residentService = residentService;
    }

    [HttpGet("tutor/{tutorId}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetResidentsByTutor(int tutorId)
    {
        var residents = await _residentService.GetResidentsByTutorAsync(tutorId);
        return Ok(residents);
    }
}