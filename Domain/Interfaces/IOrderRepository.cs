using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
    : IRepository<Order>
{
    Task SetAcceptedAsync(Guid id, Guid driverId, CancellationToken cancellationToken);

    Task SetDeliveredAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsAceeptedAsync(Guid id, Guid driverId, CancellationToken cancellationToken);

    Task<List<TModel>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken);

    Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken);
}
