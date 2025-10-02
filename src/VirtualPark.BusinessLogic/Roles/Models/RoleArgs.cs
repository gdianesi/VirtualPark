using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Roles.Models;

public sealed class RoleArgs(string name, string description, List<string> permissions, List<String> users)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public string Description { get; init; } = ValidationServices.ValidateNullOrEmpty(description);
    public List<Guid> PermissionIds { get; init; } = ValidateAndParseGuidList(permissions);
    public List<Guid> UsersIds { get; init; } = ValidateAndParseGuidList(users);

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
