using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Roles.ModelsIn;

public class CreateRoleRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? PermissionsIds { get; init; }

    public RoleArgs ToArgs()
    {
        return new RoleArgs(ValidationServices.ValidateNullOrEmpty(Name),
            ValidationServices.ValidateNullOrEmpty(Description),
            ValidationServices.ValidateList(PermissionsIds));
    }
}
