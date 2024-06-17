namespace Domain.Dtos;

public record DriverFilter : IFilter
{
    public Guid? NotifiedOrderId { get; set; }
}
