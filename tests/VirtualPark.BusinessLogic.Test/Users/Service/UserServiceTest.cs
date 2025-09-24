using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Users.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("UserService")]
public class UserServiceTest
{
    private Mock<IRepository<User>> _usersRepositoryMock = null!;
    private UserService _userService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _usersRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _userService = new UserService(_usersRepositoryMock.Object);
    }

    #region Create
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void CreateUser_ShouldCreateUser_WhenEmailDoesNotExist()
    {
        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!");

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _usersRepositoryMock
            .Setup(r => r.Add(It.Is<User>(u =>
                u.Name == args.Name &&
                u.LastName == args.LastName &&
                u.Email == args.Email &&
                u.Password == args.Password)));

        var result = _userService.Create(args);

        result.Should().NotBeEmpty();

        _usersRepositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!");

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(true);

        var act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Email already exists");

        _usersRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
