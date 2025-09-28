using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Permissions.Models;

public sealed class PermissionArgs(string description, string key, List<Guid> roles)
{
    public string Description { get; set; } = ValidationServices.ValidateNullOrEmpty(description);
    public string Key { get; set; } = ValidationServices.ValidateNullOrEmpty(key);
    public List<Guid> Roles { get; set; } = ValidateGuids(roles);

    private static List<Guid> ValidateGuids(List<Guid> ids)
    {
        if(ids == null || ids.Count == 0)
        {
            throw new ArgumentException("Roles list cannot be null or empty");
        }

        return ids;
    }
}
