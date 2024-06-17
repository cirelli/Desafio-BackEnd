using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IRentRepository : IRepository<Rent>
{
    public Task<List<TModel>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken);

    public Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken);

    public Task<Rent?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
