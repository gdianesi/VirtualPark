using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Permissions.Models;
using VirtualPark.BusinessLogic.Permissions.Service;
using VirtualPark.WebApi.Controllers.Permissions.ModelsIn;
using VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Permissions;

[ApiController]
public sealed class PermissionController(IPermissionService permissionService) : ControllerBase
{
    private readonly IPermissionService _permissionService = permissionService;

    [HttpPost("permissions")]
    public CreatePermissionResponse CreatePermission(CreatePermissionRequest request)
    {
        PermissionArgs args = request.ToArgs();

        Guid id = _permissionService.Create(args);

        return new CreatePermissionResponse(id.ToString());
    }
}
