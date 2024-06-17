namespace Domain.Interfaces;

public interface IRepositoryWrapper
{
    IDriverRepository Driver { get; }
    IMotorbikeRepository Motorbike { get; }
    IOrderRepository Order { get; }
    IRentPlanRepository RentPlan { get;  }
    IRentRepository Rent { get; }
    IUserRepository User { get; }
    IMessageRepository Message { get; }
    INotificationRepository Notification { get; }

    Task CommitAsync(CancellationToken cancellationToken);

    Task OpenTransactionAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);
}
