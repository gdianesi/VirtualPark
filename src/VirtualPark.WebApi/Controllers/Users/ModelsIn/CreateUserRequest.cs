using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

namespace VirtualPark.WebApi.Controllers.Users.ModelsIn;

public class CreateUserRequest
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public List<string>? RolesIds { get; init; }
    public CreateVisitorProfileRequest? VisitorProfile { get; init; }
}
