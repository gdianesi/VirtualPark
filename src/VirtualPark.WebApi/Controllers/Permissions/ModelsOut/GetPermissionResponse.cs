using VirtualPark.BusinessLogic.Permissions.Entity;

namespace VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

public class GetPermissionResponse(Permission permission)
{
    public string Id { get; } = permission.Id.ToString();
    public string Description { get; } = permission.Description;
    public string Key { get; } = permission.Key;
    public List<string> Roles { get; } = permission.Roles
            .Select(r => r.Id.ToString())
            .ToList();
}
