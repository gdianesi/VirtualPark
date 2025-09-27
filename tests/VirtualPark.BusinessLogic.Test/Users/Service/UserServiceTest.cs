using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Users.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("UserService")]
public class UserServiceTest
{
    private Mock<IRepository<User>> _usersRepositoryMock = null!;
    private Mock<IReadOnlyRepository<Role>> _rolesRepositoryMock = null!;
    private Mock<IVisitorProfile> _visitorProfileServiceMock = null!;
    private Mock<IReadOnlyRepository<VisitorProfile>> _visitorProfileRepositoryMock = null!;
    private UserService _userService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _usersRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _rolesRepositoryMock = new Mock<IReadOnlyRepository<Role>>(MockBehavior.Strict);
        _visitorProfileServiceMock = new Mock<IVisitorProfile>(MockBehavior.Strict);
        _visitorProfileRepositoryMock = new Mock<IReadOnlyRepository<VisitorProfile>>(MockBehavior.Strict);
        _userService = new UserService(_usersRepositoryMock.Object, _rolesRepositoryMock.Object, _visitorProfileServiceMock.Object, _visitorProfileRepositoryMock.Object);
    }

    #region Create
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void CreateUser_ShouldCreateUser_WhenEmailDoesNotExist()
    {
        var roleId = Guid.NewGuid();
        var roles = new List<string> { roleId.ToString() };

        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!", roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == roleId))
            .Returns(new Role { Name = "Visitor" });

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

    [TestMethod]
    [TestCategory("Validation")]
    public void CreateUser_ShouldCreateUser_WithVisitorProfile_FromArgs()
    {
        var roleId = Guid.NewGuid();
        var roles = new List<string> { roleId.ToString() };

        var vpArgs = new VisitorProfileArgs("2000-01-01", "Standard");
        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!", roles)
        {
            VisitorProfile = vpArgs
        };

        var vpId = Guid.NewGuid();
        var vpEntity = new VisitorProfile
        {
            Id = vpId,
            DateOfBirth = vpArgs.DateOfBirth,
            Membership = vpArgs.Membership
        };

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == roleId))
            .Returns(new Role { Name = "Visitor" });

        _visitorProfileServiceMock
            .Setup(s => s.Create(vpArgs))
            .Returns(vpEntity);

        _usersRepositoryMock
            .Setup(r => r.Add(It.Is<User>(u =>
                u.Name == args.Name &&
                u.LastName == args.LastName &&
                u.Email == args.Email &&
                u.Password == args.Password &&
                u.VisitorProfile != null &&
                u.VisitorProfileId == vpId &&
                u.VisitorProfile!.Id == vpId &&
                u.VisitorProfile!.DateOfBirth == vpArgs.DateOfBirth &&
                u.VisitorProfile!.Membership == vpArgs.Membership)));

        var result = _userService.Create(args);

        result.Should().NotBeEmpty();

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
        _visitorProfileServiceMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };

        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!", roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(true);

        var act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Email already exists");

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenRoleDoesNotExist_ThrowsInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var roles = new List<string> { id.ToString() };
        var args = new UserArgs("Ana", "Gomez", "ana@mail.com", "Password123!", roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == id))
            .Returns((Role?)null);

        Action act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Role with id {id} does not exist.");

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Get
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldReturnUserWithVisitorProfile_WhenUserExists()
    {
        var vpId = Guid.NewGuid();

        var dbUser = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = vpId
        };

        var id = dbUser.Id;

        var dbVisitorProfile = new VisitorProfile
        {
            Id = vpId,
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Visitors.Entity.Membership.Standard
        };

        _usersRepositoryMock
            .Setup(r => r.Get(u => u.Id == id))
            .Returns(dbUser);

        _visitorProfileRepositoryMock
            .Setup(r => r.Get(vp => vp.Id == dbUser.VisitorProfileId))
            .Returns(dbVisitorProfile);

        var result = _userService.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.VisitorProfileId.Should().Be(vpId);
        result.VisitorProfile.Should().NotBeNull();
        result.VisitorProfile!.Id.Should().Be(vpId);
        result.VisitorProfile.DateOfBirth.Should().Be(dbVisitorProfile.DateOfBirth);
        result.VisitorProfile.Membership.Should().Be(dbVisitorProfile.Membership);

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyNoOtherCalls();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldThrow_WhenUserDoesNotExist()
    {
        var anyId = Guid.NewGuid();

        _usersRepositoryMock
            .Setup(r => r.Get(u => u.Id == anyId))
            .Returns((User?)null);

        Action act = () => _userService.Get(anyId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User don't exist");

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyNoOtherCalls();
        _rolesRepositoryMock.VerifyNoOtherCalls();
    }
    #endregion
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ok()
    {
        var vpId1 = Guid.NewGuid();

        var user1 = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = vpId1
        };

        var usersFromRepo = new List<User> { user1 };

        var vp1 = new VisitorProfile
        {
            Id = vpId1,
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = VirtualPark.BusinessLogic.Visitors.Entity.Membership.Standard
        };

        _usersRepositoryMock
            .Setup(r => r.GetAll(null))
            .Returns(usersFromRepo);

        _visitorProfileRepositoryMock
            .Setup(r => r.Get(vp => vp.Id == user1.VisitorProfileId))
            .Returns(vp1);

        var result = _userService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var r1 = result[0];
        r1.Id.Should().Be(user1.Id);
        r1.VisitorProfileId.Should().Be(vpId1);
        r1.VisitorProfile.Should().NotBeNull();
        r1.VisitorProfile!.Id.Should().Be(vpId1);
        r1.VisitorProfile.DateOfBirth.Should().Be(vp1.DateOfBirth);
        r1.VisitorProfile.Membership.Should().Be(vp1.Membership);

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }
}
