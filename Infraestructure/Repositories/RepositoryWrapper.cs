using Microsoft.EntityFrameworkCore.Storage;

namespace Infraestructure.Repositories
{
    public class RepositoryWrapper(DataContext DataContext,
                                   IMapper Mapper)
        : IRepositoryWrapper
    {
        private IDbContextTransaction? transaction;

        private IDriverRepository? driverRepository;
        public IDriverRepository Driver
            => driverRepository ??= new DriverRepository(DataContext, Mapper);

        private IMotorbikeRepository? motorbikeRepository;
        public IMotorbikeRepository Motorbike
            => motorbikeRepository ??= new MotorbikeRepository(DataContext, Mapper);

        private IOrderRepository? orderRepository;
        public IOrderRepository Order
            => orderRepository ??= new OrderRepository(DataContext, Mapper);

        private IRentPlanRepository? rentPlanRepository;
        public IRentPlanRepository RentPlan
            => rentPlanRepository ??= new RentPlanRepository(DataContext, Mapper);

        private IRentRepository? rentRepository;
        public IRentRepository Rent
            => rentRepository ??= new RentRepository(DataContext, Mapper);

        private IUserRepository? userRepository;
        public IUserRepository User
            => userRepository ??= new UserRepository(DataContext);

        private IMessageRepository? messageRepository;
        public IMessageRepository Message
            => messageRepository ??= new MessageRepository(DataContext, Mapper);

        private INotificationRepository? notificationRepository;
        public INotificationRepository Notification
            => notificationRepository ??= new NotificationRepository(DataContext, Mapper);

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (transaction is not null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task OpenTransactionAsync(CancellationToken cancellationToken)
            => transaction = DataContext.Database.CurrentTransaction
                ?? await DataContext.Database.BeginTransactionAsync(cancellationToken);

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await DataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
