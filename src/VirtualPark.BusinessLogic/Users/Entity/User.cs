using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Users.Entity;

public sealed class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public VisitorProfile? VisitorProfile { get; set; }
    public Guid? VisitorProfileId { get; set; }
    public List<Role> Roles { get; set; } = null!;
}
