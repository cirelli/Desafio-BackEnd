#nullable disable

namespace Domain.Entities;

public record Notification
    : BaseEntity
{
    public Guid OrderId { get; set; }

    public Guid DriverId { get; set; }


    public Order Order { get; set; }

    public Driver Driver { get; set; }
}
