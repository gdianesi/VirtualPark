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
}
