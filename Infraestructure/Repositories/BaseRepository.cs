using System.Linq.Expressions;

namespace Infraestructure.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
{
    protected DataContext DataContext;
    protected DbSet<TEntity> DbSet;
    protected readonly IMapper mapper;


    public BaseRepository(DataContext repositoryContext, IMapper mapper)
    {
        DataContext = repositoryContext;
        DbSet = DataContext.Set<TEntity>();
        this.mapper = mapper;
    }


    protected IQueryable<TEntity> GetByIdQuery(Guid id)
        => GetByConditionQuery(q => q.Id == id);


    protected IQueryable<TEntity> GetAllQuery()
        => DbSet.AsNoTracking();

    protected IQueryable<TEntity> GetByConditionQuery(Expression<Func<TEntity, bool>> expression)
        => DbSet.Where(expression).AsNoTracking();

    public void Create(TEntity entity)
        => DbSet.Add(entity);
 
    public void Update(TEntity entity)
        => DbSet.Update(entity);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id).ExecuteDeleteAsync(cancellationToken);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id).AnyAsync(cancellationToken);


    protected async Task<TModel?> FirstOrDefaultAsync<TModel>(IQueryable query, CancellationToken cancellationToken)
    {
        var mappedQuery = mapper.ProjectTo<TModel>(query);
        return await mappedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<List<TModel>> ToListAsync<TModel>(IQueryable query, CancellationToken cancellationToken)
    {
        var mappedQuery = mapper.ProjectTo<TModel>(query);
        return await mappedQuery.ToListAsync(cancellationToken);
    }

    protected IQueryable<TEntity> OrderQuery(IQueryable<TEntity> source, Pagination pagination)
    {
        IQueryable<TEntity> query;

        if (!string.IsNullOrWhiteSpace(pagination.OrderBy))
        {
            query = source.OrderBy(pagination.OrderBy, pagination.Asc);
        }
        else
        {
            query = source.OrderByDescending(q => q.CreatedAt);
        }

        return query;
    }

    protected IQueryable<TEntity> PaginateQuery(IQueryable<TEntity> source, Pagination pagination)
    {
        IQueryable<TEntity> query = source;

        if (pagination.Offset > 0)
        {
            query = query.Skip(pagination.Offset);
        }
        if (pagination.Limit > 0)
        {
            query = query.Take(pagination.Limit.Value);
        }

        return query;
    }
}
