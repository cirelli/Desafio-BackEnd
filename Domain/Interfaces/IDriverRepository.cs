using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IDriverRepository : IRepository<Driver>
{
    Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken);

    Task<bool> ExistsByCnhAsync(string cnh, CancellationToken cancellationToken);

    Task<List<TModel>> GetAllAsync<TModel>(FilteredPagination<DriverFilter> pagination, CancellationToken cancellationToken);

    Task<List<Guid>> GetAllIdAvailableAsync(CancellationToken cancellationToken);

    Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken);

    Task<string?> GetNameAsync(Guid id, CancellationToken cancellationToken);

    Task<string?> GetCnpjAsync(Guid id, CancellationToken cancellationToken);

    Task<ECnhType> GetCnhTypeAsync(Guid driverId, CancellationToken cancellationToken);
}
