using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.WebApi.Controllers.Sessions.ModelsIn;

public class LogInSessionRequest
{
    public string? UserId { get; init; }

    public SessionArgs ToArgs()
    {
        var sessionArgs = new SessionArgs(ValidationServices.ValidateNullOrEmpty(UserId));

        return sessionArgs;
    }
}
