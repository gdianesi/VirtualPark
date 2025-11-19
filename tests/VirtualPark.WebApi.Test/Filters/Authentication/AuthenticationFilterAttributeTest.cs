using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Filters.Authenticator;

namespace VirtualPark.WebApi.Test.Filters.Authentication;

[TestClass]
[TestCategory("Filter")]
public class AuthenticationFilterAttributeTest
{
    #region HeaderMissing

    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenAuthorizationHeaderMissing_ShouldReturnUnauthorized()
    {
        var filter = new AuthenticationFilterAttribute();
        var context = CreateAuthorizationContext(headers: null);

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var value = result.Value!.ToString();
        value.Should().Contain("Unauthenticated");
    }

    #endregion

    private static AuthorizationFilterContext CreateAuthorizationContext(IHeaderDictionary? headers)
    {
        var httpContext = new DefaultHttpContext();
        if(headers != null)
        {
            foreach(var header in headers)
            {
                httpContext.Request.Headers[header.Key] = header.Value;
            }
        }

        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        return new AuthorizationFilterContext(actionContext, []);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenEndpointAllowsAnonymous_ShouldSkipAuthorization()
    {
        var filter = new AuthenticationFilterAttribute();

        var metadata = new List<object> { new AllowAnonymousAttribute() };
        var endpoint = new Endpoint(null, new EndpointMetadataCollection(metadata), "AllowAnonymous");

        var httpContext = new DefaultHttpContext();
        httpContext.SetEndpoint(endpoint);

        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var context = new AuthorizationFilterContext(actionContext, []);

        filter.OnAuthorization(context);

        context.Result.Should().BeNull();
    }

    #region InvalidHeaderValue

    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenAuthorizationHeaderFormatIsInvalid_ShouldReturnInvalidAuthorization()
    {
        var filter = new AuthenticationFilterAttribute();

        var headers = new HeaderDictionary { { "Authorization", "InvalidTokenFormat" } };

        var context = CreateAuthorizationContext(headers);

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var value = result.Value!.ToString();
        value.Should().Contain("InvalidAuthorization");
    }

    #endregion

    #region ExpiredToken

    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenAuthorizationTokenIsExpired_ShouldReturnExpiredAuthorization()
    {
        var filter = new AuthenticationFilterAttribute();

        var headers = new HeaderDictionary { { "Authorization", "Bearer expired-token-123" } };

        var context = CreateAuthorizationContext(headers);

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var value = result.Value!.ToString();
        value.Should().Contain("ExpiredAuthorization");
    }

    #endregion

    #region ValidTokenAuthorization

    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenAuthorizationTokenIsValid_ShouldAssignUserToContext()
    {
        var mockSessionService = new Mock<ISessionService>(MockBehavior.Strict);
        var validToken = Guid.NewGuid();
        var user = new User { Email = "user@test.com" };

        mockSessionService.Setup(s => s.GetUserLogged(validToken)).Returns(user);

        var filter = new AuthenticationFilterAttribute();

        var headers = new HeaderDictionary { { "Authorization", $"Bearer {validToken}" } };

        var context = CreateAuthorizationContext(headers);

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(mockSessionService.Object)
            .BuildServiceProvider();

        filter.OnAuthorization(context);

        context.Result.Should().BeNull("because a valid token should not block the request");
        context.HttpContext.Items.ContainsKey("UserLogged").Should().BeTrue();
        var storedUser = context.HttpContext.Items["UserLogged"] as User;
        storedUser.Should().NotBeNull();
        storedUser.Email.Should().Be("user@test.com");

        mockSessionService.VerifyAll();
    }

    #endregion

    #region InvalidTokenGuid

    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenTokenIsNotGuid_ShouldReturnInvalidAuthorization()
    {
        var filter = new AuthenticationFilterAttribute();

        var headers = new HeaderDictionary { { "Authorization", "Bearer not-a-guid" } };

        var context = CreateAuthorizationContext(headers);

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var value = result.Value!.ToString();
        value.Should().Contain("InvalidAuthorization");
    }

    #endregion

    #region SessionService
    #region Null
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenSessionServiceIsNull_ShouldReturnInternalError()
    {
        var filter = new AuthenticationFilterAttribute();

        var validToken = Guid.NewGuid();

        var headers = new HeaderDictionary { { "Authorization", $"Bearer {validToken}" } };

        var context = CreateAuthorizationContext(headers);

        context.HttpContext.RequestServices = new ServiceCollection().BuildServiceProvider();

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        result.Value!.ToString().Should().Contain("InternalError");
    }
    #endregion

    #region InvalidOperationException
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenSessionServiceThrowsInvalidOperationException_ShouldReturnExpiredAuthorization()
    {
        var mockSessionService = new Mock<ISessionService>(MockBehavior.Strict);
        var validToken = Guid.NewGuid();

        mockSessionService.Setup(s => s.GetUserLogged(validToken))
            .Throws(new InvalidOperationException("Session expired"));

        var filter = new AuthenticationFilterAttribute();

        var headers = new HeaderDictionary
        {
            { "Authorization", $"Bearer {validToken}" }
        };

        var context = CreateAuthorizationContext(headers);

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(mockSessionService.Object)
            .BuildServiceProvider();

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var value = result.Value!.ToString();
        value.Should().Contain("ExpiredAuthorization");

        mockSessionService.VerifyAll();
    }
    #endregion
    #endregion
}
