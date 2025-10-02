using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Roles.Models;

public sealed class RoleArgs(string name, string description, string[] permissions)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public string Description { get; init; } = ValidationServices.ValidateNullOrEmpty(description);
    public List<Guid> PermissionIds { get; init; } = permissions.Select(ValidationServices.ValidateAndParseGuid).ToList();
}
