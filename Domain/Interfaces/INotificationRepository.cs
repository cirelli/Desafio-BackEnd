using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface INotificationRepository
    : IRepository<Notification>
{
    Task<bool> ExistsAsync(Guid orderId, Guid driverId, CancellationToken cancellationToken);

    Task<List<Notification>> GetAllAsync(Pagination pagination, CancellationToken cancellationToken);

    Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
