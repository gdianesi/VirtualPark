using VirtualPark.BusinessLogic.Permissions.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Permissions.ModelsIn;

public class CreatePermissionRequest
{
    public string? Description { get; init; }
    public string? Key { get; init; }
    public List<string>? RolesIds { get; init; }

    public PermissionArgs ToArgs()
    {
        var permissionArgs = new PermissionArgs(ValidationServices.ValidateNullOrEmpty(Description),
            ValidationServices.ValidateNullOrEmpty(Key),
            ValidationServices.ValidateList(RolesIds));

        return permissionArgs;
    }
}
