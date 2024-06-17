using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    public Task<List<Message>> GetAllAsync(Pagination pagination, CancellationToken cancellationToken);

    public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
