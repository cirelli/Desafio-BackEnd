namespace Infraestructure.Repositories;

public class MotorbikeRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<Motorbike>(repositoryContext, mapper),
      IMotorbikeRepository
{
    public async Task<bool> ExistsByPlateAsync(string plate, CancellationToken cancellationToken)
        => await GetByConditionQuery(q => q.Plate == plate).AnyAsync(cancellationToken);

    public async Task<List<TModel>> GetAllAsync<TModel>(FilteredPagination<BaseFilter> pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        if (!string.IsNullOrEmpty(pagination.Filters.Search))
        {
            query = query.Where(q => q.Plate!.Contains(pagination.Filters.Search));
        }

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await ToListAsync<TModel>(query, cancellationToken);
    }

    public async Task<Guid> GetAvailableIdAsync(CancellationToken cancellationToken)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        return await GetByConditionQuery(q => q.Rents.Count == 0 || q.Rents.Any(r => r.EndDate < now))
            .Select(q => q.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken)
        => await FirstOrDefaultAsync<TModel>(GetByIdQuery(id),
                                             cancellationToken);

    public async Task UpdatePlateAsync(Guid id, string plate, CancellationToken cancellationToken)
        => await GetByIdQuery(id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(q => q.Plate, plate)
                                            .SetProperty(q => q.UpdatedAt, DateTime.UtcNow),
                                cancellationToken);
}
