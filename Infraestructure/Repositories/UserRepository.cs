namespace Infraestructure.Repositories;

public class UserRepository(DataContext dataContext)
      : IUserRepository
{
    public async Task<Guid?> GetDriverIdAsync(Guid id, CancellationToken cancellationToken)
        => await dataContext.Users
            .Where(q => q.Id == id)
            .Select(q => q.DriverId)
            .FirstOrDefaultAsync(cancellationToken);
}
