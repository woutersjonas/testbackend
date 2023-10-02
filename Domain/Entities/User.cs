using Microsoft.AspNetCore.Identity;

namespace jonas.Domain.Entities.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}
