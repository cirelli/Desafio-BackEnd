#nullable disable

namespace Domain.Entities;

public record Rent
    : BaseEntity
{
    public Rent()
    {
        StartDate = DateOnly.FromDateTime(CreatedAt.AddDays(1));
    }

    public Guid DriverId { get; set; }
    public Guid RentPlanId { get; set; }
    public Guid MotorbikeId { get; set; }


    public decimal Value { get; set; }
    public decimal Fee { get; set; }
    public decimal AdditionalValue { get; set; }


    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly PreviewEndDate { get; set; }


    public Driver Driver { get; set; }
    public RentPlan RentPlan { get; set; }
    public Motorbike Motorbike { get; set; }
}
