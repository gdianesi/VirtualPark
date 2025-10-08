using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Sessions;

[ApiController]
public sealed class SessionController(ISessionService sessionService) : ControllerBase
{
    private readonly ISessionService _sessionService = sessionService;

    [HttpPost("sessions/login")]
    public LogInSessionResponse LogIn([FromBody] LogInSessionRequest request)
    {
        SessionArgs args = request.ToArgs();

        Guid token = _sessionService.LogIn(args);

        return new LogInSessionResponse(token.ToString());
    }

    [HttpGet("sessions/getUser/{token}")]
    public GetUserLoggedSessionResponse GetUserLogged(string token)
    {
        var sessionToken = ValidationServices.ValidateAndParseGuid(token);

        var user = _sessionService.GetUserLogged(sessionToken);

        return new GetUserLoggedSessionResponse(user.Id.ToString());
    }

    [HttpDelete("sessions/logout/{token}")]
    public void LogOut(string token)
    {
        var sessionToken = ValidationServices.ValidateAndParseGuid(token);
        _sessionService.LogOut(sessionToken);
    }
}
