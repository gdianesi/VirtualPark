namespace VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

public class GetPermissionResponse(string id)
{
    public string Id { get; } = id;
}
