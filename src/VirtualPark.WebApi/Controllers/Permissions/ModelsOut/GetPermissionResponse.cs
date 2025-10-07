namespace VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

public class GetPermissionResponse(string id, string description, string key, List<string> roles)
{
    public string Id { get; } = id;
    public string Description { get; } = description;
    public string Key { get; } = key;
    public List<string> Roles { get; } = roles;
}
