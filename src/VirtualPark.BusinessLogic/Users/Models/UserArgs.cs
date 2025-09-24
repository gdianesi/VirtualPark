using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Users.Models;

public class UserArgs
{
    public string Name { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public VisitorProfileArgs? VisitorProfile { get; set; }

    public UserArgs(string name, string lastName, string email, string password)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Password = password;
    }
}
