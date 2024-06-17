using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IMotorbikeRepository : IRepository<Motorbike>
{
    public Task<bool> ExistsByPlateAsync(string plate, CancellationToken cancellationToken);

    public Task<List<TModel>> GetAllAsync<TModel>(FilteredPagination<BaseFilter> pagination, CancellationToken cancellationToken);

    public Task<Guid> GetAvailableIdAsync(CancellationToken cancellationToken);

    public Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken);

    public Task UpdatePlateAsync(Guid id, string plate, CancellationToken cancellationToken);
}
