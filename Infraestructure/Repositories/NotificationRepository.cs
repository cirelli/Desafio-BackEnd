namespace Infraestructure.Repositories;

public class NotificationRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<Notification>(repositoryContext, mapper),
      INotificationRepository
{
    public async Task<List<Notification>> GetAllAsync(Pagination pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> ExistsAsync(Guid orderId, Guid driverId, CancellationToken cancellationToken)
        => await GetByConditionQuery(q => q.OrderId == orderId && q.DriverId == driverId).AnyAsync(cancellationToken);
}
