using IRepository = Domain.Interfaces.INotificationRepository;
using TEntity = Domain.Entities.Notification;

namespace Services;

public class NotificationService(IRepositoryWrapper RepositoryWrapper)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.Notification;

    public async Task<ServiceResult<TEntity>> GetByIdAsync(Guid id,
                                                           CancellationToken cancellationToken)
    {
        TEntity? value = await Repository.GetByIdAsync(id, cancellationToken);

        if (value is null)
        {
            return NotFound();
        }

        return Success(value);
    }

    public async Task<ServiceResult<List<TEntity>>> GetAllAsync(Pagination pagination,
                                                                       CancellationToken cancellationToken)
    {
        pagination ??= new();

        List<TEntity> values = await Repository.GetAllAsync(pagination, cancellationToken);

        return Success(values);
    }
}
