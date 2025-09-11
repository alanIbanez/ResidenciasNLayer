using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpPost]
    [Authorize(Policy = "PreceptorOrGuardia")]
    public async Task<ActionResult<Attendance>> RegisterAttendance([FromBody] AttendanceRegistrationRequest request)
    {
        try
        {
            var attendance = await _attendanceService.RegisterAsync(
                request.ResidentUserId,
                request.Type,
                request.OccurredAt,
                request.EventId);

            return CreatedAtAction(nameof(GetAttendance), new { id = attendance.Id }, attendance);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("bulk")]
    [Authorize(Policy = "PreceptorOrGuardia")]
    public async Task<ActionResult<IEnumerable<Attendance>>> RegisterBulkAttendance([FromBody] BulkAttendanceRequest request)
    {
        try
        {
            var attendances = await _attendanceService.RegisterBulkAsync(request.Entries);
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Attendance>> GetAttendance(int id)
    {
        // This would require additional method in service to get by ID
        return NotFound("Get by ID not implemented yet");
    }

    [HttpGet("resident/{residentId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByResident(
        int residentId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var attendances = await _attendanceService.GetByResidentAsync(residentId, fromDate, toDate);
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class AttendanceRegistrationRequest
{
    public int ResidentUserId { get; set; }
    public string? Type { get; set; }
    public DateTime? OccurredAt { get; set; }
    public int? EventId { get; set; }
}

public class BulkAttendanceRequest
{
    public IEnumerable<AttendanceRegistrationDto> Entries { get; set; } = new List<AttendanceRegistrationDto>();
}