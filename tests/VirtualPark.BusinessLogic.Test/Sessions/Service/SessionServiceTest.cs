using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Sessions.Entity;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Sessions.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("SessionService")]
public class SessionServiceTest
{
    private Mock<IRepository<Session>> _sessionRepositoryMock = null!;
    private Mock<IReadOnlyRepository<User>> _userRepositoryMock = null!;
    private Mock<IVisitRegistrationService> _visitRegistrationServiceMock = null!;
    private SessionService _sessionService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sessionRepositoryMock = new Mock<IRepository<Session>>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IReadOnlyRepository<User>>(MockBehavior.Strict);
        _visitRegistrationServiceMock = new Mock<IVisitRegistrationService>(MockBehavior.Strict);
        _sessionService = new SessionService(_sessionRepositoryMock.Object, _userRepositoryMock.Object, _visitRegistrationServiceMock.Object);
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

        var email = user.Email;
        var password = user.Password;
        var args = new SessionArgs(email, password);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns(user);

        _sessionRepositoryMock
            .Setup(r => r.Add(It.Is<Session>(s =>
                s.Email == email &&
                s.Password == password &&
                s.UserId == user.Id &&
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
        var email = "noexiste@mail.com";
        var password = "Password123!";
        var args = new SessionArgs(email, password);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns((User?)null);

        var act = () => _sessionService.LogIn(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Invalid credentials.");

        _userRepositoryMock.VerifyAll();
        _sessionRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void LogIn_ShouldThrow_WhenPasswordIsIncorrect()
    {
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!"
        };

        var email = user.Email;
        var wrongPassword = "OtherPass1!";
        var args = new SessionArgs(email, wrongPassword);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns(user);

        var act = () => _sessionService.LogIn(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Invalid credentials.");

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

        var session = new Session
        {
            Email = user.Email,
            Password = user.Password,
            UserId = user.Id,
            User = user
        };
        var token = session.Token;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        var email = session.Email;

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
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
        var email = "desaparecido@mail.com";
        var session = new Session
        {
            Email = email,
            UserId = Guid.NewGuid()
        };
        var token = session.Token;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns((User?)null);

        var act = () => _sessionService.GetUserLogged(token);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Invalid credentials.");

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region LogOut
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void LogOut_ShouldRemoveSession_WhenTokenIsValidAndNoVisitorProfile()
    {
        var user = new User
        {
            Email = "pepe@mail.com",
            VisitorProfileId = null
        };

        var session = new Session
        {
            Email = user.Email,
            UserId = user.Id
        };

        var token = session.Token;
        var email = session.Email;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns(user);

        _sessionRepositoryMock
            .Setup(r => r.Remove(session))
            .Verifiable();

        _sessionService.LogOut(token);

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        _visitRegistrationServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void LogOut_ShouldCloseVisitAndRemoveSession_WhenUserHasVisitorProfile()
    {
        var visitorProfileId = Guid.NewGuid();
        var user = new User
        {
            Email = "pepe@mail.com",
            VisitorProfileId = visitorProfileId
        };

        var session = new Session
        {
            Email = user.Email,
            UserId = user.Id
        };

        var token = session.Token;
        var email = session.Email;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns(user);

        _visitRegistrationServiceMock
            .Setup(v => v.CloseVisitByVisitor(visitorProfileId))
            .Verifiable();

        _sessionRepositoryMock
            .Setup(r => r.Remove(session))
            .Verifiable();

        _sessionService.LogOut(token);

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        _visitRegistrationServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void LogOut_ShouldRemoveSession_WhenNoActiveVisitFound()
    {
        var visitorProfileId = Guid.NewGuid();
        var user = new User
        {
            Email = "pepe@mail.com",
            VisitorProfileId = visitorProfileId
        };

        var session = new Session
        {
            Email = user.Email,
            UserId = user.Id
        };

        var token = session.Token;
        var email = session.Email;

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns(session);

        _userRepositoryMock
            .Setup(r => r.Get(u => u.Email == email))
            .Returns(user);

        _visitRegistrationServiceMock
            .Setup(v => v.CloseVisitByVisitor(visitorProfileId))
            .Throws(new InvalidOperationException("No active visit found"));

        _sessionRepositoryMock
            .Setup(r => r.Remove(session))
            .Verifiable();

        _sessionService.LogOut(token);

        _sessionRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        _visitRegistrationServiceMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void LogOut_ShouldThrow_WhenSessionNotFound()
    {
        var token = Guid.NewGuid();

        _sessionRepositoryMock
            .Setup(r => r.Get(s => s.Token == token))
            .Returns((Session?)null);

        var act = () => _sessionService.LogOut(token);

        act.Should()
            .Throw<Exception>()
            .WithMessage("Session not found or the token has expired.");

        _sessionRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
