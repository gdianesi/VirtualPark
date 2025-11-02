namespace VirtualPark.WebApi.Controllers.Roles.ModelsOut;

public class GetRoleResponse(string id, string name, string description, List<string> permissionIds, List<string> usersIds)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public List<string> PermissionIds { get; } = permissionIds;
    public List<string> UsersIds { get; } = usersIds;
}
