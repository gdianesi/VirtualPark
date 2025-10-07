using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Roles.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Roles.ModelsIn;
using VirtualPark.WebApi.Controllers.Roles.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Roles;

[ApiController]
[Route("roles")]
public sealed class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpPost]
    public CreateRoleResponse CreateRole(CreateRoleRequest newRole)
    {
        RoleArgs roleArgs = newRole.ToArgs();

        Guid responseId = _roleService.Create(roleArgs);

        return new CreateRoleResponse(responseId.ToString());
    }
}
