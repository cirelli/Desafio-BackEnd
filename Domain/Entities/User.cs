using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User()
    : IdentityUser<Guid>
{
    public Guid? DriverId { get; set; }


    public Driver? Driver { get; set; }
}
