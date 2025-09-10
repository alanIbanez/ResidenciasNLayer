using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Application.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public EventService(ApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<int> CreateEventAsync(string name, string description, DateTime dateAt, int createdBy)
    {
        var eventEntity = new Event
        {
            name = name,
            description = description,
            date_at = dateAt,
            created_by = createdBy,
            createdat = DateTime.UtcNow,
            updatedat = DateTime.UtcNow
        };

        _context.events.Add(eventEntity);
        await _context.SaveChangesAsync();

        // Send notification to all residents
        var notificationTitle = "Nuevo evento";
        var notificationBody = $"{name} en {dateAt:yyyy-MM-dd HH:mm}";
        
        await _notificationService.SendNotificationToRoleAsync("residente", notificationTitle, notificationBody);

        return eventEntity.id;
    }

    public async Task<IEnumerable<dynamic>> GetEventsAsync()
    {
        return await _context.events
            .Include(e => e.createdby)
            .Select(e => new
            {
                id = e.id,
                name = e.name,
                description = e.description,
                date_at = e.date_at,
                created_by_name = $"{e.createdby.firstname} {e.createdby.lastname}",
                createdat = e.createdat
            })
            .ToListAsync();
    }
}