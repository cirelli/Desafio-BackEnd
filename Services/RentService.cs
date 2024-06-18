using Domain.Enums;
using DTO = Domain.Dtos.RentDTO;
using IRepository = Domain.Interfaces.IRentRepository;
using TEntity = Domain.Entities.Rent;

namespace Services;

public class RentService(IRepositoryWrapper RepositoryWrapper,
                         IMapper mapper,
                         IValidator<DTO> dtoValidator)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.Rent;

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

    public async Task<ServiceResult<TEntity>> CreateAsync(Guid userId,
                                                          DTO dto,
                                                          CancellationToken cancellationToken)
    {
        Guid? driverId = await RepositoryWrapper.User.GetDriverIdAsync(userId, cancellationToken);
        if (driverId is null || driverId == Guid.Empty)
        {
            return Forbidden();
        }

        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationError(validationResult);
        }

        RentPlan? rentPlan = await RepositoryWrapper.RentPlan.GetByIdAsync(dto.RentPlanId, cancellationToken);
        if (rentPlan is null)
        {
            return ValidationError(nameof(dto.RentPlanId), "Invalid!");
        }

        ECnhType cnhType = await RepositoryWrapper.Driver.GetCnhTypeAsync(driverId.Value, cancellationToken);
        if (!cnhType.HasFlag(ECnhType.A))
        {
            return Invalid("Driver does not have the required type of license");
        }

        Guid? motorbikeId = await RepositoryWrapper.Motorbike.GetAvailableIdAsync(cancellationToken);
        if (motorbikeId == Guid.Empty)
        {
            return Conflict("There are no motorbikes available!");
        }

        TEntity entity = new()
        {
            DriverId = driverId.Value,
            MotorbikeId = motorbikeId.Value,
            RentPlanId = dto.RentPlanId,
            Value = CalculatePrice(rentPlan)
        };
        entity.PreviewEndDate = entity.StartDate.AddDays(rentPlan.Days - 1);
        entity.EndDate = entity.PreviewEndDate;

        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        return Success(entity);
    }

    public async Task<ServiceResult<RentViewModel>> SetEndDateAsync(Guid userId,
                                                                    Guid id,
                                                                    RentPatchDTO dto,
                                                                    CancellationToken cancellationToken)
    {
        Guid? driverId = await RepositoryWrapper.User.GetDriverIdAsync(userId, cancellationToken);
        if (driverId is null || driverId == Guid.Empty)
        {
            return Forbidden();
        }

        var rent = await Repository.GetByIdAsync(id, cancellationToken);

        if (rent is null)
        {
            return NotFound();
        }

        if (rent.DriverId != driverId.Value)
        {
            return NotFound();
        }

        rent.EndDate = dto.EndDate;
        UpdateRentValue(ref rent);
        rent.UpdatedAt = DateTime.UtcNow;
        Repository.Update(rent);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        var value = mapper.Map<RentViewModel>(rent);
        return Success(value);
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

    private static decimal CalculatePrice(RentPlan rentPlan)
        => rentPlan.Days * rentPlan.Price;

    protected static void UpdateRentValue(ref Rent rent)
    {
        var diffDate = rent.EndDate.ToDateTime(TimeOnly.MinValue) - rent.PreviewEndDate.ToDateTime(TimeOnly.MinValue);

        if (diffDate.Days == 0)
        {
            return;
        }

        // early return
        else if (diffDate.Days < 0)
        {
            decimal unusedDailyValue = Math.Abs(diffDate.Days) * rent.RentPlan.Price;

            rent.Value -= unusedDailyValue;
            rent.Fee = unusedDailyValue * rent.RentPlan.Fee / 100;
        }

        // later return
        else
        {
            rent.AdditionalValue = diffDate.Days * rent.RentPlan.AdditionalDailyPrice;
        }
    }
}
