namespace VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

public class CreatePermissionResponse(string id)
{
    public string Id { get; } = id;
}
