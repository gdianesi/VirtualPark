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

    [TestMethod]
    [TestCategory("Validation")]
    public void Create__WhenEmailAlreadyExists__ThrowsInvalidOperationException()
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
}
