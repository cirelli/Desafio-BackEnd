using Domain.Enums;

#nullable disable

namespace Domain.Entities;

public record Order
    : BaseEntity
{
    public Guid? DriverId { get; set; }

    public decimal Value { get; set; }

    public EOrderStatus Status { get; set; }


    public Driver Driver { get; set; }
}
