namespace VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

public class GetPermissionResponse(string id, string description)
{
    public string Id { get; } = id;
    public string Description { get; } = description;
}
