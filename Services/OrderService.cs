using System.Threading;
using Notification;

using DTO = Domain.Dtos.OrderDTO;
using IRepository = Domain.Interfaces.IOrderRepository;
using TEntity = Domain.Entities.Order;

namespace Services;

public class OrderService(IRepositoryWrapper RepositoryWrapper,
                          IMapper mapper,
                          IValidator<DTO> dtoValidator,
                          Notification.NotificationService notificationService)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.Order;

    public async Task<ServiceResult<TModel>> GetByIdAsync<TModel>(Guid id,
                                                                  CancellationToken cancellationToken)
        where TModel : class
    {
        TModel? value = await Repository.GetByIdAsync<TModel>(id, cancellationToken);

        if (value is null)
        {
            return NotFound<TModel>();
        }

        return new SuccessServiceResult<TModel>(value);
    }

    public async Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(Pagination pagination,
                                                                       CancellationToken cancellationToken)
        where TModel : class
    {
        pagination ??= new();

        List<TModel> values = await Repository.GetAllAsync<TModel>(pagination, cancellationToken);

        return new SuccessServiceResult<List<TModel>>(values);
    }

    public async Task<ServiceResult<TEntity>> CreateAsync(DTO dto,
                                                          CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new FluentValidationErrorServiceResult<TEntity>(validationResult);
        }

        TEntity entity = mapper.Map<TEntity>(dto);
        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        await notificationService.ProduceAsync(NotificationKeys.OrderCreated, entity);

        return new SuccessServiceResult<TEntity>(entity);
    }

    public async Task<ServiceResult> DeleteAsync(Guid id,
                                                 CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.DeleteAsync(id, cancellationToken);

        return new SuccessServiceResult();
    }

    public async Task<ServiceResult> AcceptAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        Guid? driverId = await RepositoryWrapper.User.GetDriverIdAsync(userId, cancellationToken);
        if (driverId is null || driverId == Guid.Empty)
        {
            return new ForbiddenServiceResult<TEntity>();
        }

        if(!(await RepositoryWrapper.Notification.ExistsAsync(id, driverId.Value, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.SetAcceptedAsync(id, driverId.Value, cancellationToken);
        return new SuccessServiceResult();
    }

    public async Task<ServiceResult> DeliverAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        Guid? driverId = await RepositoryWrapper.User.GetDriverIdAsync(userId, cancellationToken);
        if (driverId is null || driverId == Guid.Empty)
        {
            return new ForbiddenServiceResult<TEntity>();
        }

        if (!(await Repository.ExistsAceeptedAsync(id, driverId.Value, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.SetDeliveredAsync(id, cancellationToken);
        return new SuccessServiceResult();
    }
}
