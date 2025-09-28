namespace VirtualPark.BusinessLogic.Permissions.Models;

public sealed class PermissionArgs(string description, string key, List<Guid> roles)
{
    public string Description { get; set; } = description;
    public string Key { get; set; } = key;
    public List<Guid> Roles { get; set; } = roles;
}
