using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;
using System.Security.Claims;

namespace ResidenciasNLayer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    [Authorize(Roles = "preceptor")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var eventId = await _eventService.CreateEventAsync(
                request.name,
                request.description,
                request.date_at,
                userId
            );

            return Ok(new { id = eventId, message = "Event created successfully" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        try
        {
            var events = await _eventService.GetEventsAsync();
            return Ok(events);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}