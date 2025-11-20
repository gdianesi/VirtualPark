using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Roles.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Roles.ModelsIn;
using VirtualPark.WebApi.Controllers.Roles.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Roles;

[ApiController]
[AuthenticationFilter]
[Route("roles")]
public sealed class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpPost]
    [AuthorizationFilter]
    public CreateRoleResponse CreateRole([FromBody] CreateRoleRequest newRole)
    {
        RoleArgs roleArgs = newRole.ToArgs();

        Guid responseId = _roleService.Create(roleArgs);

        return new CreateRoleResponse(responseId.ToString());
    }

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public GetRoleResponse GetRoleById(string id)
    {
        var roleId = ValidationServices.ValidateAndParseGuid(id);

        var role = _roleService.Get(roleId)!;

        return new GetRoleResponse(role);
    }

    [HttpGet]
    [AuthorizationFilter]
    public List<GetRoleResponse> GetAllRoles()
    {
        return _roleService
            .GetAll()
            .Select(r => new GetRoleResponse(r))
            .ToList();
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    public void DeleteRole(string id)
    {
        var roleId = ValidationServices.ValidateAndParseGuid(id);
        _roleService.Remove(roleId);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    public void UpdateRole(CreateRoleRequest request, string id)
    {
        var roleId = ValidationServices.ValidateAndParseGuid(id);

        RoleArgs args = request.ToArgs();

        _roleService.Update(args, roleId);
    }
}
