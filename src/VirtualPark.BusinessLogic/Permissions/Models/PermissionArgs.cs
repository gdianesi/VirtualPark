using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Permissions.Models;

public sealed class PermissionArgs(string description, string key, List<Guid> roles)
{
    public string Description { get; set; } = ValidationServices.ValidateNullOrEmpty(description);
    public string Key { get; set; } = ValidateNullOrEmpty(key);
    public List<Guid> Roles { get; set; } = roles;

    public static string ValidateNullOrEmpty(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        return value;
    }
}
