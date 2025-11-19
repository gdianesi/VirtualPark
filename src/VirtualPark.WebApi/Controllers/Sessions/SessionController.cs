using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;

namespace VirtualPark.WebApi.Controllers.Sessions;

[AuthenticationFilter]
[ApiController]
[Route("sessions")]
public sealed class SessionController(ISessionService sessionService) : ControllerBase
{
    private readonly ISessionService _sessionService = sessionService;

    [AllowAnonymous]
    [HttpPost("sessions")]
    public LogInSessionResponse LogIn([FromBody] LogInSessionRequest request)
    {
        SessionArgs args = request.ToArgs();

        Guid token = _sessionService.LogIn(args);

        return new LogInSessionResponse(token.ToString());
    }

    [HttpGet("sessions/me")]
    public GetUserLoggedSessionResponse GetUserLogged()
    {
        var user = (User)HttpContext.Items["UserLogged"];
        var roleNames = user.Roles.Select(r => r.Name).ToList();

        return new GetUserLoggedSessionResponse(user.Id.ToString(), user.VisitorProfileId.ToString(), roleNames);
    }

    [HttpDelete("sessions")]
    public void LogOut()
    {
        var tokenString = HttpContext.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", string.Empty)
            .Trim();

        var token = Guid.Parse(tokenString);

        _sessionService.LogOut(token);
    }
}
