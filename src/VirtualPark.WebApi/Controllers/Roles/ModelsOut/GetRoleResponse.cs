namespace VirtualPark.WebApi.Controllers.Roles.ModelsOut;

public class GetRoleResponse(string id, string name, string description, List<string> permissionIds, List<string> usersIds)
{
    public string Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;
    public List<string> PermissionIds { get; init; } = permissionIds;
    public List<string> UsersIds { get; init; } = usersIds;
}
