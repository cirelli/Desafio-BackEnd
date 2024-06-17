namespace Infraestructure.Repositories;

public class MessageRepository(DataContext repositoryContext, IMapper mapper)
    : BaseRepository<Message>(repositoryContext, mapper),
      IMessageRepository
{
    public async Task<List<Message>> GetAllAsync(Pagination pagination, CancellationToken cancellationToken)
    {
        var query = GetAllQuery();

        query = OrderQuery(query, pagination);
        query = PaginateQuery(query, pagination);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
}
