namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<Guid?> GetDriverIdAsync(Guid id, CancellationToken cancellationToken);
}
