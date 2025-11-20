using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.WebApi.Controllers.Roles.ModelsOut;

public class GetRoleResponse(Role role)
{
    public string Id { get; } = role.Id.ToString();
    public string Name { get; } = role.Name;
    public string Description { get; } = role.Description;
    public List<string> PermissionIds { get; } = role.Permissions
            .Select(p => p.Id.ToString())
            .ToList();
    public List<string> UsersIds { get; } = role.Users
            .Select(u => u.Id.ToString())
            .ToList();
}
