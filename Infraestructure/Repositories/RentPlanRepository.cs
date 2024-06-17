namespace Infraestructure.Repositories;

public class RentPlanRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<RentPlan>(repositoryContext, mapper),
      IRentPlanRepository
{
    public async Task<List<TModel>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await ToListAsync<TModel>(query, cancellationToken);
    }

    public async Task<RentPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken)
        => await FirstOrDefaultAsync<TModel>(GetByIdQuery(id), cancellationToken);
}
