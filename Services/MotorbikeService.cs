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
            return NotFound();
        }

        return Success(value);
    }

    public async Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(FilteredPagination<BaseFilter> pagination, CancellationToken cancellationToken)
        where TModel : class
    {
        pagination ??= new();

        List<TModel> values = await Repository.GetAllAsync<TModel>(pagination, cancellationToken);

        return Success(values);
    }

    public async Task<ServiceResult<TEntity>> CreateAsync(DTO dto, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationError(validationResult);
        }

        if (await Repository.ExistsByPlateAsync(dto.Plate!, cancellationToken))
        {
            return Conflict("Motorbike already registered!");
        }

        TEntity entity = mapper.Map<TEntity>(dto);
        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        return Success(entity);
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
            return ValidationError(validationResult);
        }

        if (await Repository.ExistsByPlateAsync(dto.Plate!, cancellationToken))
        {
            return Conflict("Plate already in use!");
        }

        await Repository.UpdatePlateAsync(id, dto.Plate!, cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.DeleteAsync(id, cancellationToken);
        return Success();
    }
}
