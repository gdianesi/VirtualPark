using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using VirtualPark.BusinessLogic.Sessions.Service;

namespace VirtualPark.WebApi.Filters.Authenticator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];

        if(string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = BuildErrorResult("Unauthenticated", "You are not authenticated");
            return;
        }

        if(!authorizationHeader.ToString().StartsWith("Bearer "))
        {
            context.Result = BuildErrorResult("InvalidAuthorization", "The provided authorization header format is invalid");
            return;
        }

        if(IsAuthorizationExpired(authorizationHeader.ToString() ?? string.Empty))
        {
            context.Result = BuildErrorResult("ExpiredAuthorization", "The provided authorization header is expired");
            return;
        }

        try
        {
            var tokenString = authorizationHeader.ToString().Replace("Bearer ", string.Empty).Trim();
            var token = Guid.Parse(tokenString);

            var sessionService = context.HttpContext.RequestServices.GetService<ISessionService>();

            if(sessionService is null)
            {
                context.Result = BuildErrorResult("InternalError", "Session service not available");
                return;
            }

            var user = sessionService.GetUserLogged(token);
            context.HttpContext.Items["UserLogged"] = user;
        }
        catch(FormatException)
        {
            context.Result = BuildErrorResult("InvalidAuthorization", "Invalid token format");
        }
        catch(InvalidOperationException)
        {
            context.Result = BuildErrorResult("ExpiredAuthorization", "The provided authorization header is expired");
        }

        return;
    }

    private static bool IsAuthorizationExpired(string authorization)
    {
        return authorization.Contains("expired", StringComparison.OrdinalIgnoreCase);
    }

    private static ObjectResult BuildErrorResult(string innerCode, string message) =>
        new(new { InnerCode = innerCode, Message = message })
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
}
