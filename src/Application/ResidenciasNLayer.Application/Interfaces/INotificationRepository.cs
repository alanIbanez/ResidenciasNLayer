using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.Application.Interfaces;

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification);
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, bool onlyUnread = false);
    Task<Notification> UpdateAsync(Notification notification);
}