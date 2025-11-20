using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Users.Models;

public class UserArgs(string name, string lastName, string email, string? password, List<string> roles)
{
    public string Name { get; } = name;
    public string LastName { get; } = lastName;
    public string Email { get; } = ValidationServices.ValidateEmail(email);
    public string? Password { get; } = password is null
        ? null
        : ValidationServices.ValidatePassword(password);
    public VisitorProfileArgs? VisitorProfile { get; set; }
    public List<Guid> RolesIds { get; } = ValidateAndParseGuidList(roles);

    private static List<Guid> ValidateAndParseGuidList(List<string> values)
    {
        var result = new List<Guid>();
        foreach(var value in values)
        {
            result.Add(ValidationServices.ValidateAndParseGuid(value));
        }

        return result;
    }
}
