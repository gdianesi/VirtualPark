using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Roles.ModelsIn;

public class CreateRoleRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
}
