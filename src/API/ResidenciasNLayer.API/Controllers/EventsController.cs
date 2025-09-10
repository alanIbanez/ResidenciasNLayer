using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.Interfaces;

namespace ResidenciasNLayer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;

    public EventsController(INotificationService notificationService, IUserRepository userRepository)
    {
        _notificationService = notificationService;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Authorize(Policy = "PreceptorOrGuardia")]
    public async Task<ActionResult> PublishEvent([FromBody] PublishEventDto request)
    {
        try
        {
            // In a real implementation, you would create/save the event first
            var eventId = new Random().Next(1000, 9999); // Mock event ID

            // Get recipients based on request criteria
            var allUsers = await _userRepository.GetAllAsync();
            var recipients = allUsers.Where(u => u.IsActive);

            if (request.TargetRoles?.Any() == true)
            {
                recipients = recipients.Where(u => request.TargetRoles.Contains(u.Role.Name));
            }

            await _notificationService.NotifyEventPublishedAsync(eventId, recipients);

            return Ok(new { eventId, message = "Event published and notifications sent", recipientCount = recipients.Count() });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class PublishEventDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? EventDate { get; set; }
    public string[]? TargetRoles { get; set; } // Optional: filter recipients by roles
}