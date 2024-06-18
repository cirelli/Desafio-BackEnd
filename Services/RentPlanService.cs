using DTO = Domain.Dtos.RentPlanDTO;
using IRepository = Domain.Interfaces.IRentPlanRepository;
using TEntity = Domain.Entities.RentPlan;

namespace Services;

public class RentPlanService(IRepositoryWrapper RepositoryWrapper,
                           IMapper mapper,
                           IValidator<DTO> dtoValidator)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.RentPlan;

    public async Task<ServiceResult<TModel>> GetByIdAsync<TModel>(Guid id,
                                                                  CancellationToken cancellationToken)
        where TModel : class
    {
        TModel? value = await Repository.GetByIdAsync<TModel>(id, cancellationToken);

        if (value is null)
        {
            return NotFound();
        }

        return Success(value);
    }

    public async Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(Pagination pagination,
                                                                       CancellationToken cancellationToken)
        where TModel : class
    {
        pagination ??= new();

        List<TModel> values = await Repository.GetAllAsync<TModel>(pagination, cancellationToken);

        return Success(values);
    }

    public async Task<ServiceResult<TEntity>> CreateAsync(DTO dto,
                                                           CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationError(validationResult);
        }

        TEntity entity = mapper.Map<TEntity>(dto);
        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        return Success(entity);
    }

    public async Task<ServiceResult> DeleteAsync(Guid id,
                                                 CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.DeleteAsync(id, cancellationToken);

        return Success();
    }
}
