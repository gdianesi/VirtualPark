using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        result!.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

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

        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

    #region InvalidHeaderValue
    [TestMethod]
    [TestCategory("Behaviour")]
    public void OnAuthorization_WhenAuthorizationHeaderFormatIsInvalid_ShouldReturnInvalidAuthorization()
    {
        var filter = new AuthenticationFilterAttribute();

        var headers = new HeaderDictionary
        {
            { "Authorization", "InvalidTokenFormat" }
        };

        var context = CreateAuthorizationContext(headers);

        filter.OnAuthorization(context);

        var result = context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var value = result.Value!.ToString();
        value.Should().Contain("InvalidAuthorization");
    }
    #endregion
}
