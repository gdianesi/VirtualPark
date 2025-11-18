using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Permissions.Models;
using VirtualPark.BusinessLogic.Permissions.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Permissions.ModelsIn;
using VirtualPark.WebApi.Controllers.Permissions.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Permissions;

[ApiController]
[AuthenticationFilter]
[Route("permissions")]
public sealed class PermissionController(IPermissionService permissionService) : ControllerBase
{
    private readonly IPermissionService _permissionService = permissionService;

    [HttpPost]
    [AuthorizationFilter]
    public CreatePermissionResponse CreatePermission(CreatePermissionRequest request)
    {
        PermissionArgs args = request.ToArgs();

        Guid id = _permissionService.Create(args);

        return new CreatePermissionResponse(id.ToString());
    }

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public GetPermissionResponse GetPermissionById(string id)
    {
        var permissionId = ValidationServices.ValidateAndParseGuid(id);

        var permission = _permissionService.GetById(permissionId)!;

        return new GetPermissionResponse(
            id: permission.Id.ToString(),
            description: permission.Description,
            key: permission.Key,
            roles: permission.Roles.Select(r => r.Id.ToString()).ToList());
    }

    [HttpGet]
    [AuthorizationFilter]
    public List<GetPermissionResponse> GetAllPermissions()
    {
        return _permissionService.GetAll()
            .Select(p => new GetPermissionResponse(
                id: p.Id.ToString(),
                description: p.Description,
                key: p.Key,
                roles: p.Roles.Select(r => r.Id.ToString()).ToList()))
            .ToList();
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    public void DeletePermission(string id)
    {
        var permissionId = ValidationServices.ValidateAndParseGuid(id);
        _permissionService.Remove(permissionId);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    public void UpdatePermission(CreatePermissionRequest request, string id)
    {
        var permissionId = ValidationServices.ValidateAndParseGuid(id);

        PermissionArgs args = request.ToArgs();

        _permissionService.Update(permissionId, args);
    }
}
