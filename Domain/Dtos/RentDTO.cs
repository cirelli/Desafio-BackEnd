namespace Domain.Dtos;

public record RentDTO(Guid RentPlanId);

public record RentPatchDTO(DateOnly EndDate);

public record RentViewModel
{
    public Guid Id { get; set; }
    public Guid RentPlanId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly PreviewEndDate { get; set; }
    public decimal Value { get; set; }
    public decimal Fee { get; set; }
    public decimal AdditionalValue { get; set; }
    public decimal Total { get; set; }
}
