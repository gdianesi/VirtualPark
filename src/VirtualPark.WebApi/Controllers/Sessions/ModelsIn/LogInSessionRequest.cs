using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Sessions.ModelsIn;

public class LogInSessionRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }

    public SessionArgs ToArgs()
    {
        var sessionArgs = new SessionArgs(ValidationServices.ValidateNullOrEmpty(Email), ValidationServices.ValidateNullOrEmpty(Password));

        return sessionArgs;
    }
}
