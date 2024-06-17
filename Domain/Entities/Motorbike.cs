#nullable disable

namespace Domain.Entities;

public record Motorbike
    : BaseEntity
{
    public string Plate { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }


    public List<Rent> Rents { get; set; }
}