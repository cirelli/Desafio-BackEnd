namespace Domain.Dtos;

public record OrderDTO(decimal Value);

public record OrderViewModel(Guid Id, DateTime CreatedAt, decimal Value, string Status);
