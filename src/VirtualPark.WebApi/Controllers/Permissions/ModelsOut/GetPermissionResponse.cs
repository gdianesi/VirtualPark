namespace VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

public class GetPermissionResponse(string id, string description, string key)
{
    public string Id { get; } = id;
    public string Description { get; } = description;
    public string Key { get; } = key;
}
