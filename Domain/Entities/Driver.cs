using Domain.Enums;

#nullable disable

namespace Domain.Entities;

public record Driver
    : BaseEntity
{
    public string Name { get; set; }

    public string Cnpj { get; set; }

    public DateOnly BirthDate { get; set; }

    public string Cnh { get; set; }

    public ECnhType CnhType { get; set; }

    public string CnhImage { get; set; }


    public User User { get; set; }

    public List<Rent> Rents { get; set; }

    public List<Order> Orders { get; set; }

    public List<Notification> Notifications { get; set; }
}
