using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Permissions.Models;

public sealed class PermissionArgs(string description, string key, List<string> roles)
{
    public string Description { get; } = ValidationServices.ValidateNullOrEmpty(description);
    public string Key { get; } = ValidationServices.ValidateNullOrEmpty(key);
    public List<Guid> Roles { get; } = ValidateAndParseGuidList(roles);

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
