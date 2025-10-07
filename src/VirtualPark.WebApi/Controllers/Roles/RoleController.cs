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

    [HttpGet("roles/{id}")]
    public GetRoleResponse GetRoleById(string id)
    {
        var roleId = ValidationServices.ValidateAndParseGuid(id);

        var role = _roleService.Get(roleId)!;

        return new GetRoleResponse(
            id: role.Id.ToString(),
            name: role.Name,
            description: role.Description,
            permissionIds: role.Permissions.Select(p => p.Id.ToString()).ToList(),
            usersIds: role.Users.Select(u => u.Id.ToString()).ToList());
    }

    [HttpGet]
    public List<GetRoleResponse> GetAllRoles()
    {
        return _roleService.GetAll()
            .Select(r => new GetRoleResponse(
                id: r.Id.ToString(),
                name: r.Name,
                description: r.Description,
                permissionIds: r.Permissions.Select(p => p.Id.ToString()).ToList(),
                usersIds: r.Users.Select(u => u.Id.ToString()).ToList()))
            .ToList();
    }

    [HttpDelete("roles/{id}")]
    public void DeleteRole(string id)
    {
        var roleId = ValidationServices.ValidateAndParseGuid(id);
        _roleService.Remove(roleId);
    }

    [HttpPut("roles/{id}")]
    public void UpdateRole(CreateRoleRequest request, string id)
    {
        var roleId = ValidationServices.ValidateAndParseGuid(id);

        RoleArgs args = request.ToArgs();

        _roleService.Update(args, roleId);
    }
}
