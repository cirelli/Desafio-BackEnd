using Microsoft.AspNetCore.Identity;

using DTO = Domain.Dtos.DriverDTO;
using IRepository = Domain.Interfaces.IDriverRepository;
using TEntity = Domain.Entities.Driver;

namespace Services;

public class DriverService(IRepositoryWrapper RepositoryWrapper,
                           IMapper mapper,
                           IValidator<DTO> dtoValidator,
                           UserManager<User> userManager)
    : BaseService
{
    public IRepository Repository => RepositoryWrapper.Driver;

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

    public async Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(FilteredPagination<DriverFilter> pagination,
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

        if (await Repository.ExistsByCnpjAsync(dto.Cnpj!, cancellationToken))
        {
            return Conflict("CNPJ already registered!");
        }

        if (await Repository.ExistsByCnhAsync(dto.Cnh!, cancellationToken))
        {
            return Conflict("CNH already registered!");
        }

        TEntity entity = mapper.Map<TEntity>(dto);

        await RepositoryWrapper.OpenTransactionAsync(cancellationToken);
        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        var user = new User { UserName = dto.Cnpj, DriverId = entity.Id };
        IdentityResult result = await userManager.CreateAsync(user, dto.Password!);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Driver");

            await RepositoryWrapper.CommitAsync(cancellationToken);
            return Success(entity);
        }

        await RepositoryWrapper.RollbackAsync(cancellationToken);
        return ValidationError(result.Errors.Select(q => new KeyValuePair<string, string>(nameof(dto.Password), q.Description)));
    }

    public async Task<ServiceResult> DeleteAsync(Guid id,
                                                 CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        string? userName = await Repository.GetCnpjAsync(id, cancellationToken);
        User? user = await userManager.FindByNameAsync(userName!);
        await userManager.DeleteAsync(user!);
        await Repository.DeleteAsync(id, cancellationToken);

        return Success();
    }
}
