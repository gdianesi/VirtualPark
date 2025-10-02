using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Roles.Models;

public sealed class RoleArgs(string name, string description, List<string> permissions)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public string Description { get; init; } = ValidationServices.ValidateNullOrEmpty(description);
    public List<Guid> PermissionIds { get; init; } = ValidateAndParseGuidList(permissions);

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
