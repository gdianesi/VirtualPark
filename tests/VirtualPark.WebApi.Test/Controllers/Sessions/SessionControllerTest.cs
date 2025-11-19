using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Controllers.Sessions;
using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Sessions;

[TestClass]
public class SessionControllerTest
{
    private Mock<ISessionService> _sessionServiceMock = null!;
    private SessionController _sessionController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
        _sessionController = new SessionController(_sessionServiceMock.Object);
    }

    #region LogIn
    [TestMethod]
    public void LogIn_ValidInput_ReturnsTokenResponse()
    {
        var request = new LogInSessionRequest { Email = "email@gmail.com", Password = "Password!1" };

        var expectedArgs = request.ToArgs();
        var token = Guid.NewGuid();

        _sessionServiceMock
            .Setup(s => s.LogIn(It.Is<SessionArgs>(a =>
                a.Email == expectedArgs.Email &&
                a.Password == expectedArgs.Password)))
            .Returns(token);

        var response = _sessionController.LogIn(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<LogInSessionResponse>();
        response.Token.Should().Be(token.ToString());

        _sessionServiceMock.VerifyAll();
    }
    #endregion

    #region GetUserLogged
    [TestMethod]
    public void GetUserLogged_ValidToken_ReturnsUserId()
    {
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = []
        };

        var httpContext = new DefaultHttpContext { Items = { ["UserLogged"] = user } };

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var response = _sessionController.GetUserLogged();

        response.Should().NotBeNull();
        response.Id.Should().Be(user.Id.ToString());
        response.Roles.Should().BeEmpty();
    }

    [TestMethod]
    public void GetUserLogged_ShouldThrow_WhenUserLoggedNotPresent()
    {
        var httpContext = new DefaultHttpContext();

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        Action act = () => _sessionController.GetUserLogged();

        act.Should().Throw<NullReferenceException>();

        _sessionServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetUserLogged_WithRoles_ShouldReturnRoleNames()
    {
        var role = new Role { Name = "Administrator" };
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = [role]
        };

        var httpContext = new DefaultHttpContext { Items = { ["UserLogged"] = user } };

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var response = _sessionController.GetUserLogged();

        response.Id.Should().Be(user.Id.ToString());
        response.Roles.Should().BeEquivalentTo(["Administrator"]);
    }

    [TestMethod]
    public void GetUserLogged_WhenUserHasVisitorProfile_ShouldReturnVisitorId()
    {
        var visitorId = Guid.NewGuid();

        var user = new User
        {
            Name = "Ana",
            LastName = "Lopez",
            Email = "ana@mail.com",
            Password = "Password123!",
            VisitorProfileId = visitorId,
            Roles = []
        };

        var httpContext = new DefaultHttpContext { Items = { ["UserLogged"] = user } };

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var response = _sessionController.GetUserLogged();

        response.VisitorId.Should().Be(visitorId.ToString());
    }

    [TestMethod]
    public void GetUserLogged_WhenNoVisitorProfile_ShouldReturnEmptyVisitorId()
    {
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = null,
            Roles = []
        };

        var httpContext = new DefaultHttpContext { Items = { ["UserLogged"] = user } };

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var res = _sessionController.GetUserLogged();

        res.VisitorId.Should().Be(null);
        res.Roles.Should().BeEmpty();
    }

    [TestMethod]
    public void GetUserLogged_WithMultipleRoles_ShouldReturnAllRoleNames()
    {
        var r1 = new Role { Name = "Admin" };
        var r2 = new Role { Name = "Manager" };

        var user = new User
        {
            Name = "Ana",
            LastName = "Gomez",
            Email = "ana@mail.com",
            Password = "Password123!",
            Roles = [r1, r2]
        };

        var httpContext = new DefaultHttpContext { Items = { ["UserLogged"] = user } };

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var res = _sessionController.GetUserLogged();

        res.Should().NotBeNull();
        res.Id.Should().Be(user.Id.ToString());
        res.Roles.Should().BeEquivalentTo(["Admin", "Manager"]);
    }

    #endregion

    #region LogOut
    [TestMethod]
    public void LogOut_ValidToken_ShouldCallServiceLogOut()
    {
        var token = Guid.NewGuid();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = $"Bearer {token}";

        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _sessionServiceMock
            .Setup(s => s.LogOut(token));

        _sessionController.LogOut();

        _sessionServiceMock.VerifyAll();
    }

    [TestMethod]
    public void LogOut_ShouldThrowFormatException_WhenAuthorizationHeaderIsMissing()
    {
        var httpContext = new DefaultHttpContext();
        _sessionController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        Action act = _sessionController.LogOut;

        act.Should().Throw<FormatException>();

        _sessionServiceMock.VerifyNoOtherCalls();
    }

    #endregion
}
