using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Roles.Models;

public sealed class RoleArgs(string name)
{
    public string Name { get; set; } = ValidationServices.ValidateNullOrEmpty(name);
}
