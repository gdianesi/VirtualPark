using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Service;

namespace VirtualPark.WebApi.Filters.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilterAttribute(string? permission = null)
    : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(context.Result != null)
        {
            return;
        }

        if(context.HttpContext.Items["UserLogged"] is not User userLogged)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthorized",
                Message = "Not authenticated"
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        var requiredPermission =
            permission ?? $"{context.RouteData.Values["action"]}-{context.RouteData.Values["controller"]}";

        var userService = context.HttpContext.RequestServices.GetService<IUserService>();
        if(userService is null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError",
                Message = "User service not available"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return;
        }

        var hasPermission = userService.HasPermission(userLogged.Id, requiredPermission);

        if(!hasPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"Missing permission {requiredPermission}"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
