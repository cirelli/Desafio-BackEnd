namespace Infraestructure.Repositories;

public class RentRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<Rent>(repositoryContext, mapper),
      IRentRepository
{
    public async Task<List<TModel>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await ToListAsync<TModel>(query, cancellationToken);
    }

    public async Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken)
        => await FirstOrDefaultAsync<TModel>(GetByIdQuery(id),
                                             cancellationToken);

    public async Task<Rent?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id)
            .Include(q => q.RentPlan)
            .FirstOrDefaultAsync(cancellationToken);
}
