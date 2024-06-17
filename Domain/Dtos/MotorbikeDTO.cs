namespace Domain.Dtos;

public record MotorbikeDTO(string? Plate, int Year, string? Model) : MotorbikePatchDTO(Plate);

public record MotorbikePatchDTO(string? Plate);

public record MotorbikeViewModel(Guid Id, string Plate, int Year, string Model);