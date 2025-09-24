using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Users.Models;

public class UserArgs
{
    public string Name { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public VisitorProfileArgs? VisitorProfile { get; init; }
}
