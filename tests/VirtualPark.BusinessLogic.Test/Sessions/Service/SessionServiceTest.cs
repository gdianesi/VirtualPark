using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Sessions.Entity;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Sessions.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("SessionService")]
public class SessionServiceTest
{
    private Mock<IRepository<Session>> _sessionRepositoryMock = null!;
    private Mock<IReadOnlyRepository<User>> _userRepositoryMock = null!;
    private SessionService _sessionService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sessionRepositoryMock = new Mock<IRepository<Session>>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IReadOnlyRepository<User>>(MockBehavior.Strict);
        _sessionService = new SessionService(_sessionRepositoryMock.Object, _userRepositoryMock.Object);
    }

    #region LogIn
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void LogIn_ShouldAddSession_WhenUserExists()
    {
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!"
        };
        var userId = user.Id;
        var args = new SessionArgs(userId.ToString());

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Id == userId))
            .Returns(user);

        _sessionRepositoryMock
            .Setup(r => r.Add(It.Is<Session>(s =>
                s.UserId == userId &&
                s.User == user)));

        var result = _sessionService.LogIn(args);

        result.Should().NotBeEmpty();

        _userRepositoryMock.VerifyAll();
        _sessionRepositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void LogIn_ShouldThrow_WhenUserDoesNotExist()
    {
        var nonExistingUserId = Guid.NewGuid();
        var args = new SessionArgs(nonExistingUserId.ToString());

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Id == nonExistingUserId))
            .Returns((User?)null);

        var act = () => _sessionService.LogIn(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User not exist.");

        _userRepositoryMock.VerifyAll();
        _sessionRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region GetUserLogged
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void GetUserLogged_ShouldReturnUser_WhenSessionAndUserExist()
    {
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = []
        };
        var userId = user.Id;

        var session = new Session
        {
            UserId = userId,
            User = user
        };

        var token = session.Token;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Id == userId))
            .Returns(user);

        var result = _sessionService.GetUserLogged(token);

        result.Should().NotBeNull();
        result.Should().BeSameAs(user);

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void GetUserLogged_ShouldThrow_WhenSessionNotFound()
    {
        var token = Guid.NewGuid();

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns((Session?)null);

        var act = () => _sessionService.GetUserLogged(token);

        act.Should()
            .Throw<Exception>()
            .WithMessage("Session not found or the token has expired.");

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetUserLogged_ShouldThrow_WhenUserNotFound()
    {
        var userId = Guid.NewGuid();

        var session = new Session
        {
            UserId = userId
        };

        var token = session.Token;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Id == userId))
            .Returns((User?)null);

        var act = () => _sessionService.GetUserLogged(token);

        act.Should()
            .Throw<Exception>()
            .WithMessage("User not exist.");

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region LogOut
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void LogOut_ShouldRemoveSession_WhenTokenIsValid()
    {
        var session = new Session
        {
            UserId = Guid.NewGuid()
        };

        var token = session.Token;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _sessionRepositoryMock
            .Setup(r => r.Remove(session))
            .Verifiable();

        _sessionService.LogOut(token);

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
