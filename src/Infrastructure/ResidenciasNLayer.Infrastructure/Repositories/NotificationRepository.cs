using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ResidenciasDbContext _context;

    public NotificationRepository(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> AddAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, bool onlyUnread = false)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId);

        if (onlyUnread)
        {
            query = query.Where(n => !n.IsRead);
        }

        return await query
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<Notification> UpdateAsync(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
        return notification;
    }
}