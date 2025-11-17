using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Sessions;

[ApiController]
[Route("sessions")]
public sealed class SessionController(ISessionService sessionService) : ControllerBase
{
    private readonly ISessionService _sessionService = sessionService;

    [HttpPost("login")]
    public LogInSessionResponse LogIn([FromBody] LogInSessionRequest request)
    {
        SessionArgs args = request.ToArgs();

        Guid token = _sessionService.LogIn(args);

        return new LogInSessionResponse(token.ToString());
    }

    [HttpGet("getUser/{token}")]
    public GetUserLoggedSessionResponse GetUserLogged(string token)
    {
        var sessionToken = ValidationServices.ValidateAndParseGuid(token);

        var user = _sessionService.GetUserLogged(sessionToken);
        var roleNames = user.Roles.Select(r => r.Name).ToList();

        return new GetUserLoggedSessionResponse(user.Id.ToString(), user.VisitorProfileId.ToString(), roleNames);
    }

    [HttpDelete("logout/{token}")]
    public void LogOut(string token)
    {
        var sessionToken = ValidationServices.ValidateAndParseGuid(token);
        _sessionService.LogOut(sessionToken);
    }
}
