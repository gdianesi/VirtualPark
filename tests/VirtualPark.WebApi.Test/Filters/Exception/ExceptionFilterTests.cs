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
    private readonly ExceptionFilter _attribute;

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
            []);
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

    #region InvalidOperation
    [TestMethod]
    public void OnException_WhenInvalidOperationException_ShouldReturnBadRequest_InvalidOperation()
    {
        _context.Exception = new InvalidOperationException("Business rule failed");

        _attribute.OnException(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(result.Value!).Should().Be("InvalidOperation");
        GetMessage(result.Value!).Should().Contain("Business rule failed");
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

    #region FormatError
    [TestMethod]
    public void OnException_WhenFormatException_ShouldReturnBadRequest_FormatError()
    {
        _context.Exception = new FormatException("The value 'abc' is not a valid GUID.");

        _attribute.OnException(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(result.Value!).Should().Be("FormatError");
        GetMessage(result.Value!).Should().Contain("not a valid GUID");
    }
    #endregion

    #region OutOfRange
    [TestMethod]
    public void OnException_WhenArgumentOutOfRangeException_ShouldReturnBadRequest_OutOfRange()
    {
        _context.Exception = new ArgumentOutOfRangeException("age", "Age must be between 1 and 99.");

        _attribute.OnException(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(result.Value!).Should().Be("OutOfRange");
        GetMessage(result.Value!).Should().Contain("Age must be between 1 and 99.");
    }
    #endregion

    #region NotFound
    [TestMethod]
    public void OnException_WhenKeyNotFoundException_ShouldReturnNotFound()
    {
        _context.Exception = new KeyNotFoundException("User doesn't exist");

        _attribute.OnException(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        GetInnerCode(result.Value!).Should().Be("NotFound");
        GetMessage(result.Value!).Should().Contain("User doesn't exist");
    }
    #endregion

    #region Unauthorized
    [TestMethod]
    public void OnException_WhenUnauthorizedAccessException_ShouldReturnUnauthorized()
    {
        _context.Exception = new UnauthorizedAccessException("Invalid credentials.");

        _attribute.OnException(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetInnerCode(result.Value!).Should().Be("Unauthorized");
        GetMessage(result.Value!).Should().Contain("Invalid credentials.");
    }
    #endregion

    private string GetInnerCode(object value)
    {
        return value.GetType().GetProperty("InnerCode").GetValue(value).ToString();
    }

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message").GetValue(value).ToString();
    }
}
