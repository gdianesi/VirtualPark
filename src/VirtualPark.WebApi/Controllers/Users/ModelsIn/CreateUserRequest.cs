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
    public UserArgs ToArgs()
    {
        var userArgs = new UserArgs(ValidationServices.ValidateNullOrEmpty(Name),
            ValidationServices.ValidateNullOrEmpty(LastName),
            ValidationServices.ValidateNullOrEmpty(Email),
            ValidationServices.ValidateNullOrEmpty(Password),
            ValidateRolesList(RolesIds));

        if(VisitorProfile != null)
        {
            userArgs.VisitorProfile = VisitorProfile.ToArgs();
        }

        return userArgs;
    }

    private List<string> ValidateRolesList(List<string>? rolesIds)
    {
        if(rolesIds == null || rolesIds.Count == 0)
        {
            throw new InvalidOperationException("Role list can't be null");
        }

        foreach(var r in rolesIds)
        {
            ValidationServices.ValidateNullOrEmpty(r);
        }

        return rolesIds;
    }
}
