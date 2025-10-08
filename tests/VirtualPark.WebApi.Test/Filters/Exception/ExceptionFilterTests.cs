using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using VirtualPark.WebApi.Filters.Exception;

namespace VirtualPark.WebApi.Test.Filters.Exception;

[TestClass]
public class ExceptionFilterTests
{
    private ExceptionContext? _context;
    private ExceptionFilter _attribute;

    public ExceptionFilterTests()
    {
        _attribute = new ExceptionFilter();
    }

    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    #region OnException
    [TestMethod]
    public void OnException_WhenExceptionIsNotRegistered_ShouldResponseInternalError()
    {
        _context.Exception = new System.Exception("Not registered");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        if(concreteResponse.Value != null)
        {
            GetInnerCode(concreteResponse.Value).Should().Be("InternalError");
            GetMessage(concreteResponse.Value).Should().Be("There was an error when processing the request");
        }
    }
    #endregion

    #region IsNullOrWhiteSpace
    [TestMethod]
    public void OnException_WhenArgumentException_ShouldReturnBadRequest_IsNullOrWhiteSpace()
    {
        _context.Exception = new ArgumentException("Value cannot be null, empty or whitespace.", "name");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(concreteResponse.Value!).Should().Be("IsNullOrWhiteSpace");
        GetMessage(concreteResponse.Value!).Should().Contain("Value cannot be null, empty or whitespace.");
    }
    #endregion

    [TestMethod]
    public void OnException_WhenArgumentOutOfRangeException_ShouldReturnBadRequest_OutOfRange()
    {
        _context.Exception = new ArgumentOutOfRangeException("age", "Age must be between 1 and 99.");

        _attribute.OnException(_context);

        var response = _context.Result;
        response.Should().NotBeNull();

        var concrete = response as ObjectResult;
        concrete.Should().NotBeNull();
        concrete!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(concrete.Value!).Should().Be("OutOfRange");
        GetMessage(concrete.Value!).Should().Contain("Age must be between 1 and 99.");
    }

    private string GetInnerCode(object value)
    {
        return value.GetType().GetProperty("InnerCode").GetValue(value).ToString();
    }

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message").GetValue(value).ToString();
    }
}
