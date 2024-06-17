namespace Domain.Interfaces;

public interface IRepository<T>
{
    void Create(T entity);

    void Update(T entity);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
