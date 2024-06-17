#nullable disable

namespace Domain.Entities;

public record RentPlan
    : BaseEntity
{
    public int Days { get; set; }

    public decimal Price { get; set; }

    public decimal Fee { get; set; }

    public decimal AdditionalDailyPrice { get; set; }


    public List<Rent> Rents { get; set; }
}
