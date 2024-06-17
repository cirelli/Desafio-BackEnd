namespace Domain.Dtos;

public record RentPlanDTO(int Days, decimal Price, decimal Fee, decimal AdditionalDailyPrice);

public record RentPlanViewModel(Guid Id, int Days, decimal Price, decimal Fee, decimal AdditionalDailyPrice);
