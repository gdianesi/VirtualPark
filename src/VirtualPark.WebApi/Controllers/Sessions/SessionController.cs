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
    [HttpPost]
    public LogInSessionResponse LogIn([FromBody] LogInSessionRequest request)
    {
        SessionArgs args = request.ToArgs();

        Guid token = _sessionService.LogIn(args);

        return new LogInSessionResponse(token.ToString());
    }

    [HttpGet("me")]
    public GetUserLoggedSessionResponse GetUserLogged()
    {
        var user = (User)HttpContext.Items["UserLogged"];
        return new GetUserLoggedSessionResponse(user);
    }

    [HttpDelete]
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
