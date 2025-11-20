using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.Users;
using VirtualPark.WebApi.Controllers.Users.ModelsIn;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Users;

[TestClass]
public class UserControllerTest
{
    private Mock<IUserService> _userServiceMock = null!;
    private UserController _usersController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _usersController = new UserController(_userServiceMock.Object);
    }

    #region Create
    [TestMethod]
    public void CreateUser_ValidInput_ReturnsCreatedUserResponse()
    {
        var role1 = Guid.NewGuid().ToString();
        var role2 = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreateUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            RolesIds = [role1, role2],
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = "2002-07-30",
                Membership = "Standard",
                Score = "85"
            }
        };

        var expectedArgs = request.ToArgs();

        _userServiceMock
            .Setup(s => s.Create(It.Is<UserArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.LastName == expectedArgs.LastName &&
                a.Email == expectedArgs.Email &&
                a.Password == expectedArgs.Password &&
                a.RolesIds.Count == expectedArgs.RolesIds.Count &&
                a.RolesIds.All(expectedArgs.RolesIds.Contains) &&
                a.VisitorProfile != null &&
                a.VisitorProfile!.DateOfBirth == expectedArgs.VisitorProfile!.DateOfBirth &&
                a.VisitorProfile.Membership == expectedArgs.VisitorProfile.Membership &&
                a.VisitorProfile.Score == expectedArgs.VisitorProfile.Score)))
            .Returns(returnId);

        var response = _usersController.CreateUser(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateUserResponse>();
        response.Id.Should().Be(returnId.ToString());

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void CreateUser_ShouldWork_WhenVisitorProfileIsNull()
    {
        var roleId = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid();

        var request = new CreateUserRequest
        {
            Name = "Juan",
            LastName = "Gomez",
            Email = "juan@mail.com",
            Password = "Secret123!",
            RolesIds = [roleId],
            VisitorProfile = null
        };

        var expectedArgs = request.ToArgs();

        _userServiceMock
            .Setup(s => s.Create(It.Is<UserArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.LastName == expectedArgs.LastName &&
                a.Email == expectedArgs.Email &&
                a.Password == expectedArgs.Password &&
                a.RolesIds.SequenceEqual(expectedArgs.RolesIds) &&
                a.VisitorProfile == null)))
            .Returns(userId);

        var result = _usersController.CreateUser(request);

        result.Should().NotBeNull();
        result.Id.Should().Be(userId.ToString());

        _userServiceMock.VerifyAll();
    }
    #endregion

    #region Get
    [TestMethod]
    public void GetUserById_ValidInput_ReturnsGetUserResponse()
    {
        var visitorProfile = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2002, 07, 30),
            Membership = Membership.Standard,
            Score = 100
        };

        var role = new Role { Name = "Visitor", Description = "Visitante" };

        var roleId = role.Id;

        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles =
            [
                role
            ],
            VisitorProfile = visitorProfile,
            VisitorProfileId = visitorProfile.Id
        };

        var id = user.Id;

        _userServiceMock
            .Setup(s => s.Get(id))
            .Returns(user);

        var response = _usersController.GetUserById(id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetUserResponse>();
        response.Id.Should().Be(id.ToString());
        response.Name.Should().Be("Pepe");
        response.LastName.Should().Be("Perez");
        response.Email.Should().Be("pepe@mail.com");

        response.Roles.Should().HaveCount(1);
        response.Roles.Should().Contain([roleId.ToString()]);

        response.VisitorProfileId.Should().Be(visitorProfile.Id.ToString());

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetUserById_ShouldReturnUserWithoutVisitorProfile_WhenProfileIsNull()
    {
        var role = new Role { Name = "Admin" };
        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = [role],
            VisitorProfile = null,
            VisitorProfileId = null
        };

        _userServiceMock
            .Setup(s => s.Get(user.Id))
            .Returns(user);

        var result = _usersController.GetUserById(user.Id.ToString());

        result.Should().NotBeNull();
        result.VisitorProfileId.Should().BeNull();

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetUserById_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";

        Action act = () => _usersController.GetUserById(invalidId);

        act.Should().Throw<FormatException>();
        _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetUserById_ShouldReturnUserWithMultipleRoles()
    {
        var role1 = new Role { Name = "Admin" };
        var role2 = new Role { Name = "Manager" };

        var user = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = [role1, role2],
            VisitorProfile = null,
            VisitorProfileId = null
        };

        _userServiceMock
            .Setup(s => s.Get(user.Id))
            .Returns(user);

        var result = _usersController.GetUserById(user.Id.ToString());

        result.Roles.Should().HaveCount(2);
        result.Roles.Should().Contain(role1.Id.ToString());
        result.Roles.Should().Contain(role2.Id.ToString());

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetUserById_ShouldReturnUser_WhenRolesIsNull()
    {
        var vp = new VisitorProfile { Membership = Membership.Standard };
        var user = new User
        {
            Name = "Nora",
            LastName = "Ruiz",
            Email = "nora@mail.com",
            Password = "Password123!",
            Roles = null,
            VisitorProfile = vp,
            VisitorProfileId = vp.Id
        };

        _userServiceMock
            .Setup(s => s.Get(user.Id))
            .Returns(user);

        var res = _usersController.GetUserById(user.Id.ToString());

        res.Should().NotBeNull();
        res.Id.Should().Be(user.Id.ToString());
        res.Name.Should().Be("Nora");
        res.LastName.Should().Be("Ruiz");
        res.Email.Should().Be("nora@mail.com");
        res.Roles.Should().NotBeNull().And.BeEmpty();
        res.VisitorProfileId.Should().Be(vp.Id.ToString());

        _userServiceMock.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAllUsers_ShouldMapUser_WhenRolesIsNull()
    {
        var user = new User
        {
            Name = "Leo",
            LastName = "Sosa",
            Email = "leo@mail.com",
            Password = "Password123!",
            Roles = null,
            VisitorProfile = null,
            VisitorProfileId = null
        };

        _userServiceMock
            .Setup(s => s.GetAll())
            .Returns([user]);

        var list = _usersController.GetAllUsers();

        list.Should().HaveCount(1);
        var first = list.First();
        first.Id.Should().Be(user.Id.ToString());
        first.Name.Should().Be("Leo");
        first.LastName.Should().Be("Sosa");
        first.Email.Should().Be("leo@mail.com");
        first.Roles.Should().NotBeNull().And.BeEmpty();
        first.VisitorProfileId.Should().BeNull();

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllUsers_ShouldReturnMappedList()
    {
        var vp1 = new VisitorProfile { Membership = Membership.Standard };
        var vp2 = new VisitorProfile { Membership = Membership.Premium };

        var role = new Role { Name = "Admin" };
        var roleId = role.Id.ToString();

        var user1 = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = [role],
            VisitorProfile = vp1,
            VisitorProfileId = vp1.Id
        };

        var role2 = new Role { Name = "Visitor" };
        var roleId2 = role2.Id.ToString();
        var user2 = new User
        {
            Name = "Ana",
            LastName = "Gomez",
            Email = "ana@mail.com",
            Password = "Password123!",
            Roles = [role2],
            VisitorProfile = vp2,
            VisitorProfileId = vp2.Id
        };

        var users = new List<User> { user1, user2 };

        _userServiceMock
            .Setup(s => s.GetAll())
            .Returns(users);

        var result = _usersController.GetAllUsers();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Name.Should().Be("Pepe");
        first.LastName.Should().Be("Perez");
        first.Email.Should().Be("pepe@mail.com");
        first.Roles.Should().ContainSingle().Which.Should().Be(roleId);
        first.VisitorProfileId.Should().Be(vp1.Id.ToString());

        var second = result.Last();
        second.Name.Should().Be("Ana");
        second.LastName.Should().Be("Gomez");
        second.Email.Should().Be("ana@mail.com");
        second.Roles.Should().ContainSingle().Which.Should().Be(roleId2);
        second.VisitorProfileId.Should().Be(vp2.Id.ToString());

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllUsers_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        _userServiceMock
            .Setup(s => s.GetAll())
            .Returns([]);

        var result = _usersController.GetAllUsers();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllUsers_ShouldMapUser_WhenVisitorProfileIsNull()
    {
        var user = new User
        {
            Name = "Lara",
            LastName = "Diaz",
            Email = "lara@mail.com",
            Password = "Password123!",
            Roles = [new Role { Name = "Admin" }],
            VisitorProfile = null,
            VisitorProfileId = null
        };

        _userServiceMock
            .Setup(s => s.GetAll())
            .Returns([user]);

        var result = _usersController.GetAllUsers();

        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var first = result.First();
        first.Id.Should().Be(user.Id.ToString());
        first.Name.Should().Be("Lara");
        first.LastName.Should().Be("Diaz");
        first.Email.Should().Be("lara@mail.com");
        first.Roles.Should().NotBeEmpty();
        first.VisitorProfileId.Should().BeNull();

        _userServiceMock.VerifyAll();
    }
    #endregion

    #region Delete
    [TestMethod]
    public void DeleteUser_ShouldRemoveUser_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _userServiceMock
            .Setup(s => s.Remove(id))
            .Verifiable();

        _usersController.DeleteUser(id.ToString());

        _userServiceMock.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    public void UpdateUser_ValidInput_ShouldCallServiceUpdate()
    {
        var id = Guid.NewGuid();
        var role1 = Guid.NewGuid().ToString();
        var role2 = Guid.NewGuid().ToString();

        var request = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [role1, role2],
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = "2002-07-30",
                Membership = "Standard",
                Score = "85"
            }
        };

        var expectedArgs = request.ToArgs();

        _userServiceMock
            .Setup(s => s.Update(It.Is<UserArgs>(a =>
                    a.Name == expectedArgs.Name &&
                    a.LastName == expectedArgs.LastName &&
                    a.Email == expectedArgs.Email &&
                    a.Password == expectedArgs.Password &&
                    a.RolesIds.Count == expectedArgs.RolesIds.Count &&
                    a.RolesIds.All(expectedArgs.RolesIds.Contains) &&
                    a.VisitorProfile != null &&
                    a.VisitorProfile!.DateOfBirth == expectedArgs.VisitorProfile!.DateOfBirth &&
                    a.VisitorProfile.Membership == expectedArgs.VisitorProfile.Membership &&
                    a.VisitorProfile.Score == expectedArgs.VisitorProfile.Score),
                id))
            .Verifiable();

        _usersController.UpdateUser(request, id.ToString());

        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void UpdateUser_ShouldWork_WhenVisitorProfileIsNull()
    {
        var id = Guid.NewGuid();
        var roleId = Guid.NewGuid().ToString();

        var request = new EditUserRequest
        {
            Name = "Mario",
            LastName = "Lopez",
            Email = "mario@mail.com",
            RolesIds = [roleId],
            VisitorProfile = null
        };

        var expectedArgs = request.ToArgs();

        _userServiceMock
            .Setup(s => s.Update(It.Is<UserArgs>(a =>
                    a.Name == expectedArgs.Name &&
                    a.LastName == expectedArgs.LastName &&
                    a.Email == expectedArgs.Email &&
                    a.Password == expectedArgs.Password &&
                    a.RolesIds.SequenceEqual(expectedArgs.RolesIds) &&
                    a.VisitorProfile == null),
                id))
            .Verifiable();

        _usersController.UpdateUser(request, id.ToString());

        _userServiceMock.VerifyAll();
    }
    #endregion
}
