using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using VirtualPark.WebApi.Filters.ResponseNormalization;

namespace VirtualPark.WebApi.Test.Filters.ResponseNormalization;

[TestClass]
public class ResponseNormalizationFilterTests
{
    private ActionExecutedContext? _context;
    private readonly ResponseNormalizationFilter _filter;

    public ResponseNormalizationFilterTests()
    {
        _filter = new ResponseNormalizationFilter();
    }

    private ActionExecutedContext CreateContext(
        string method,
        IActionResult? initialResult,
        System.Exception? exception = null)
    {
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request.Method).Returns(method);

        var actionContext = new ActionContext(
            httpContextMock.Object,
            new RouteData(),
            new ActionDescriptor());

        return new ActionExecutedContext(actionContext, [], null)
        {
            Result = initialResult,
            Exception = exception
        };
    }

    #region Post
    [TestMethod]
    public void OnActionExecuted_PostWithObjectResult_ShouldSetStatus201()
    {
        var initial = new ObjectResult("body") { StatusCode = null };
        _context = CreateContext("POST", initial);

        _filter.OnActionExecuted(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [TestMethod]
    public void OnActionExecuted_PostWithEmptyResult_ShouldSetStatus204()
    {
        _context = CreateContext("POST", new EmptyResult());

        _filter.OnActionExecuted(_context);

        var result = _context.Result as StatusCodeResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
    #endregion

    #region Put
    [TestMethod]
    public void OnActionExecuted_PutWithNullBody_ShouldSetStatus204()
    {
        var initial = new ObjectResult(null);
        _context = CreateContext("PUT", initial);

        _filter.OnActionExecuted(_context);

        var result = _context.Result as StatusCodeResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [TestMethod]
    public void OnActionExecuted_PutWithBody_ShouldStay200()
    {
        var initial = new ObjectResult("updated") { StatusCode = null };
        _context = CreateContext("PUT", initial);

        _filter.OnActionExecuted(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void OnActionExecuted_DeleteWithEmptyResult_ShouldSet204()
    {
        _context = CreateContext("DELETE", new EmptyResult());

        _filter.OnActionExecuted(_context);

        var result = _context.Result as StatusCodeResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [TestMethod]
    public void OnActionExecuted_DeleteWithBody_ShouldStay200()
    {
        var initial = new ObjectResult("removed") { StatusCode = null };
        _context = CreateContext("DELETE", initial);

        _filter.OnActionExecuted(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    #endregion

    #region Get
    [TestMethod]
    public void OnActionExecuted_GetWithBody_ShouldSet200()
    {
        var initial = new ObjectResult("data") { StatusCode = null };
        _context = CreateContext("GET", initial);

        _filter.OnActionExecuted(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [TestMethod]
    public void OnActionExecuted_GetWithStatusAlreadySet_ShouldNotChangeStatus()
    {
        var initial = new ObjectResult("data") { StatusCode = 202 };
        _context = CreateContext("GET", initial);

        _filter.OnActionExecuted(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(202);
    }
    #endregion

    #region Exception
    [TestMethod]
    public void OnActionExecuted_WhenExceptionPresent_ShouldNotModifyResult()
    {
        var initial = new ObjectResult("x") { StatusCode = 200 };
        _context = CreateContext("POST", initial, new System.Exception("fail"));

        _filter.OnActionExecuted(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
    }
    #endregion
    #region OnActionExecuting
    [TestMethod]
    public void OnActionExecuting_ShouldDoNothing()
    {
        var httpContext = new DefaultHttpContext();

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        var executingContext = new ActionExecutingContext(
            actionContext,
            [],
            new Dictionary<string, object>(),
            controller: null);

        var filter = new ResponseNormalizationFilter();
        Action act = () => filter.OnActionExecuting(executingContext);

        act.Should().NotThrow();
    }
    #endregion

}
