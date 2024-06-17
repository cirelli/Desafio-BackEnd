using Domain.Enums;

namespace Infraestructure.Repositories;

public class OrderRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<Order>(repositoryContext, mapper),
      IOrderRepository
{
    public Task<bool> ExistsAceeptedAsync(Guid id, Guid driverId, CancellationToken cancellationToken)
        => GetByConditionQuery(q => q.Id == id && q.DriverId == driverId && q.Status == EOrderStatus.Accepted).AnyAsync(cancellationToken);

    public async Task<List<TModel>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await ToListAsync<TModel>(query, cancellationToken);
    }

    public async Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken)
        => await FirstOrDefaultAsync<TModel>(GetByIdQuery(id), cancellationToken);

    public async Task SetAcceptedAsync(Guid id, Guid driverId, CancellationToken cancellationToken)
    {
        await GetByConditionQuery(q => q.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(q => q.DriverId, driverId)
                                            .SetProperty(q => q.Status, EOrderStatus.Accepted), cancellationToken);
    }

    public async Task SetDeliveredAsync(Guid id, CancellationToken cancellationToken)
    {
        await GetByConditionQuery(q => q.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(q => q.Status, EOrderStatus.Delivered), cancellationToken);
    }
}
