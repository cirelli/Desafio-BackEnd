using Domain.Enums;

namespace Infraestructure.Repositories;

public class DriverRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<Driver>(repositoryContext, mapper),
      IDriverRepository
{
    public async Task<bool> ExistsByCnhAsync(string cnh, CancellationToken cancellationToken)
        => await GetByConditionQuery(q => q.Cnh == cnh)
            .AnyAsync(cancellationToken);

    public async Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken)
        => await GetByConditionQuery(q => q.Cnpj == cnpj)
            .AnyAsync(cancellationToken);

    public async Task<List<TModel>> GetAllAsync<TModel>(FilteredPagination<DriverFilter> pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        if (pagination.Filters.NotifiedOrderId is not null && pagination.Filters.NotifiedOrderId != Guid.Empty)
        {
            query = query.Where(q => q.Notifications.Any(n => n.OrderId == pagination.Filters.NotifiedOrderId));
        }

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await ToListAsync<TModel>(query, cancellationToken);
    }

    public async Task<List<Guid>> GetAllIdAvailableAsync(CancellationToken cancellationToken)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        return await GetByConditionQuery(q => q.Rents.Any(r => r.EndDate >= now) && !q.Orders.Any(o => o.Status == EOrderStatus.Accepted))
            .Select(q => q.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<TModel?> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken)
        => await FirstOrDefaultAsync<TModel>(GetByIdQuery(id),
                                             cancellationToken);

    public async Task<string?> GetNameAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id).Select(q => q.Name)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<string?> GetCnpjAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id).Select(q => q.Cnpj)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<ECnhType> GetCnhTypeAsync(Guid id, CancellationToken cancellationToken)
        => GetByIdQuery(id).Select(q => q.CnhType)
            .FirstOrDefaultAsync(cancellationToken);
}
