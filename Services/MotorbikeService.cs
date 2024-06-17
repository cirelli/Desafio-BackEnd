using DTO = Domain.Dtos.MotorbikeDTO;
using IRepository = Domain.Interfaces.IMotorbikeRepository;
using TEntity = Domain.Entities.Motorbike;

namespace Services;

public class MotorbikeService(IRepositoryWrapper RepositoryWrapper,
                               IMapper mapper,
                               IValidator<DTO> dtoValidator,
                               IValidator<MotorbikePatchDTO> motorbikePatchValidator)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.Motorbike;

    public async Task<ServiceResult<TModel>> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken)
        where TModel : class
    {
        TModel? value = await Repository.GetByIdAsync<TModel>(id, cancellationToken);

        if (value is null)
        {
            return NotFound<TModel>();
        }

        return new SuccessServiceResult<TModel>(value);
    }

    public async Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(FilteredPagination<BaseFilter> pagination, CancellationToken cancellationToken)
        where TModel : class
    {
        pagination ??= new();

        List<TModel> values = await Repository.GetAllAsync<TModel>(pagination, cancellationToken);

        return new SuccessServiceResult<List<TModel>>(values);
    }

    public async Task<ServiceResult<TEntity>> CreateAsync(DTO dto, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new FluentValidationErrorServiceResult<TEntity>(validationResult);
        }

        if (await Repository.ExistsByPlateAsync(dto.Plate!, cancellationToken))
        {
            return new ConflictServiceResult<TEntity>("Motorbike already registered!");
        }

        TEntity entity = mapper.Map<TEntity>(dto);
        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        return new SuccessServiceResult<TEntity>(entity);
    }

    public async Task<ServiceResult> UpdatePlateAsync(Guid id, MotorbikePatchDTO dto, CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        ValidationResult validationResult = await motorbikePatchValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new FluentInvalidServiceResult(validationResult);
        }

        if (await Repository.ExistsByPlateAsync(dto.Plate!, cancellationToken))
        {
            return new ConflictServiceResult<TEntity>("Plate already in use!");
        }

        await Repository.UpdatePlateAsync(id, dto.Plate!, cancellationToken);

        return new SuccessServiceResult();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.DeleteAsync(id, cancellationToken);
        return new SuccessServiceResult();
    }
}
