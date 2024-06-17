using Domain.Enums;

#nullable disable

namespace Domain.Entities;

public record Message
    : BaseEntity
{
    public string Key { get; set; }

    public string Value { get; set; }
}
