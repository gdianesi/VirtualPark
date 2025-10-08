using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Test.Filters.Authorization;

[TestClass]
[TestCategory("Filter")]
public sealed class AuthorizationFilterAttributeTest
{
    #region Permission
    #region HasNot
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenUserHasNotPermission_ShouldReturnForbidden()
    {
        var mockUserService = new Mock<IUserService>(MockBehavior.Strict);
        var user = new User { Email = "test@virtualpark.com", Roles = [] };

        mockUserService
            .Setup(s => s.HasPermission(user.Id, "Attraction-Create"))
            .Returns(false);

        var filter = new AuthorizationFilterAttribute("Attraction-Create");

        AuthorizationFilterContext context = CreateAuthorizationContext();
        context.HttpContext.Items["UserLogged"] = user;

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(mockUserService.Object)
            .BuildServiceProvider();

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        result.Value!.ToString().Should().Contain("Missing permission Attraction-Create");

        mockUserService.VerifyAll();
    }
    #endregion
    #region Has
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenUserHasPermission_ShouldAllowRequest()
    {
        var mockUserService = new Mock<IUserService>(MockBehavior.Strict);
        var user = new User
        {
            Email = "authorized@virtualpark.com"
        };

        mockUserService
            .Setup(s => s.HasPermission(user.Id, "Attraction-Create"))
            .Returns(true);

        var filter = new AuthorizationFilterAttribute("Attraction-Create");

        var context = CreateAuthorizationContext();
        context.HttpContext.Items["UserLogged"] = user;

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(mockUserService.Object)
            .BuildServiceProvider();

        filter.OnAuthorization(context);

        context.Result.Should().BeNull("because the user has permission and the request should continue");
        mockUserService.VerifyAll();
    }
    #endregion
    #endregion

    private static AuthorizationFilterContext CreateAuthorizationContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ControllerActionDescriptor()
        };

        return new AuthorizationFilterContext(actionContext, []);
    }

    #region ServiceNull
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenUserServiceIsNull_ShouldReturnInternalError()
    {
        var filter = new AuthorizationFilterAttribute("Attraction-Create");

        var user = new User
        {
            Email = "test@virtualpark.co"
        };

        var context = CreateAuthorizationContext();
        context.HttpContext.Items["UserLogged"] = user;

        context.HttpContext.RequestServices = new ServiceCollection().BuildServiceProvider();

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var value = result.Value!.ToString();
        value.Should().Contain("User service not available");
    }
    #endregion

    #region HasResult
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenContextAlreadyHasResult_ShouldReturnImmediately()
    {
        var filter = new AuthorizationFilterAttribute("Any");
        var context = CreateAuthorizationContext();

        context.Result = new ObjectResult("PreexistingResult");

        filter.OnAuthorization(context);

        context.Result.Should().BeOfType<ObjectResult>();
        ((ObjectResult)context.Result!).Value.Should().Be("PreexistingResult");
    }
    #endregion

    #region UserLogged
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenUserNotLogged_ShouldReturnUnauthorized()
    {
        var filter = new AuthorizationFilterAttribute("Attraction-Create");
        var context = CreateAuthorizationContext();

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        result.Value!.ToString().Should().Contain("Not authenticated");
    }
    #endregion
}
