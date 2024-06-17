using Domain.Enums;

namespace Domain.Dtos;

public record DriverDTO(string? Name, string? Password, string? Cnpj, DateOnly BirthDate, string? Cnh, ECnhType CnhType);

public record DriverViewModel(Guid Id, string Name, string Cnpj, DateOnly BirthDate, string Cnh, string CnhType);
