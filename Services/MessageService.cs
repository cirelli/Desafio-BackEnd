using IRepository = Domain.Interfaces.IMessageRepository;
using TEntity = Domain.Entities.Message;

namespace Services;

public class MessageService(IRepositoryWrapper RepositoryWrapper)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.Message;

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
