using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IRentPlanRepository : IRepository<RentPlan>
{
    Task<List<TModel>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken);

    Task<RentPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken);
}
