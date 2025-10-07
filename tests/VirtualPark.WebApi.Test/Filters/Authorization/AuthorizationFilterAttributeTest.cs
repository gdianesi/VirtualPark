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
        result!.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        result.Value!.ToString().Should().Contain("Missing permission Attraction-Create");

        mockUserService.VerifyAll();
    }

    private static AuthorizationFilterContext CreateAuthorizationContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ControllerActionDescriptor()
        };

        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

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
        result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var value = result.Value!.ToString();
        value.Should().Contain("User service not available");
    }
}
