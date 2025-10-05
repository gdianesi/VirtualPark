using Moq;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.WebApi.Controllers.Users.ModelsIn;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Users;

[TestClass]
public class UserControllersTest
{
    private Mock<IUserService> _userServiceMock = null!;
    private UsersController _usersController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _usersController = new UsersController(_userServiceMock.Object);
    }

    [TestMethod]
    public void CreateUser()
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

        var response = _usersController.Create(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateUserResponse>();
        response.Id.Should().Be(returnId.ToString());

        _userServiceMock.VerifyAll();
    }
}
