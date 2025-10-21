using FluentAssertions;
using Moq;
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
        var token = Guid.NewGuid();
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!"
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        var response = _sessionController.GetUserLogged(token.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetUserLoggedSessionResponse>();
        response.Id.Should().Be(user.Id.ToString());

        _sessionServiceMock.VerifyAll();
    }
    #endregion

    #region LogOut
    [TestMethod]
    public void LogOut_ValidToken_ShouldCallServiceLogOut()
    {
        var token = Guid.NewGuid();

        _sessionServiceMock
            .Setup(s => s.LogOut(token));

        _sessionController.LogOut(token.ToString());

        _sessionServiceMock.VerifyAll();
    }
    #endregion
}
