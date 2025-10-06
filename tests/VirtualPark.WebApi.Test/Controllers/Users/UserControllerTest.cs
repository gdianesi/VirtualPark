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
            RolesIds = new List<string> { role1, role2 },
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = "2002-07-30", Membership = "Standard", Score = "85"
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
    #endregion

    [TestMethod]
    public void GetUserById()
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
            Roles = new List<Role>
            {
                role
            },
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
        response.Roles.Should().Contain(new[] { roleId.ToString() });

        response.VisitorProfileId.Should().Be(visitorProfile.Id.ToString());

        _userServiceMock.VerifyAll();
    }
}
