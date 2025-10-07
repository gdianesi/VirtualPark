using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Models;
using VirtualPark.BusinessLogic.Permissions.Service;
using VirtualPark.WebApi.Controllers.Permissions;
using VirtualPark.WebApi.Controllers.Permissions.ModelsIn;
using VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Permissions;

[TestClass]
public class PermissionControllerTest
{
    private Mock<IPermissionService> _permissionServiceMock = null!;
    private PermissionController _permissionController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _permissionServiceMock = new Mock<IPermissionService>(MockBehavior.Strict);
        _permissionController = new PermissionController(_permissionServiceMock.Object);
    }

    #region Create
    [TestMethod]
    public void CreatePermission_ValidInput_ReturnsCreatePermissionResponse()
    {
        var role1 = Guid.NewGuid().ToString();
        var role2 = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreatePermissionRequest
        {
            Description = "Manage users",
            Key = "USERS_MANAGE",
            RolesIds = [role1, role2]
        };

        var expectedArgs = request.ToArgs();

        _permissionServiceMock
            .Setup(s => s.Create(It.Is<PermissionArgs>(a =>
                a.Description == expectedArgs.Description &&
                a.Key == expectedArgs.Key &&
                a.Roles.Count == expectedArgs.Roles.Count &&
                a.Roles.All(expectedArgs.Roles.Contains))))
            .Returns(returnId);

        var response = _permissionController.CreatePermission(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreatePermissionResponse>();
        response.Id.Should().Be(returnId.ToString());

        _permissionServiceMock.VerifyAll();
    }
    #endregion
}
