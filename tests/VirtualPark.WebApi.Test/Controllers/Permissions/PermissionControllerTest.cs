using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Entity;
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

    #region GetById
    [TestMethod]
    public void GetPermissionById_ValidInput_ReturnsGetPermissionResponse()
    {
        var role1 = new BusinessLogic.Roles.Entity.Role { Name = "Admin" };
        var role2 = new BusinessLogic.Roles.Entity.Role { Name = "Visitor" };
        var permission = new Permission
        {
            Description = "Manage users",
            Key = "USERS_MANAGE",
            Roles = [role1, role2]
        };

        _permissionServiceMock
            .Setup(s => s.GetById(permission.Id))
            .Returns(permission);

        var result = _permissionController.GetPermissionById(permission.Id.ToString());

        result.Should().NotBeNull();
        result.Should().BeOfType<GetPermissionResponse>();
        result.Id.Should().Be(permission.Id.ToString());
        result.Description.Should().Be("Manage users");
        result.Key.Should().Be("USERS_MANAGE");
        result.Roles.Should().HaveCount(2);
        result.Roles.Should().Contain(role1.Id.ToString());
        result.Roles.Should().Contain(role2.Id.ToString());

        _permissionServiceMock.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAllPermissions_ShouldReturnMappedList()
    {
        var role1 = new BusinessLogic.Roles.Entity.Role { Name = "Admin" };
        var role2 = new BusinessLogic.Roles.Entity.Role { Name = "Visitor" };

        var permission1 = new Permission
        {
            Description = "Manage Users",
            Key = "USERS_MANAGE",
            Roles = [role1]
        };

        var permission2 = new Permission
        {
            Description = "View Stats",
            Key = "STATS_VIEW",
            Roles = [role2]
        };

        _permissionServiceMock
            .Setup(s => s.GetAll())
            .Returns([permission1, permission2]);

        var result = _permissionController.GetAllPermissions();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Description.Should().Be("Manage Users");
        first.Key.Should().Be("USERS_MANAGE");
        first.Roles.Should().ContainSingle().Which.Should().Be(role1.Id.ToString());

        var second = result.Last();
        second.Description.Should().Be("View Stats");
        second.Key.Should().Be("STATS_VIEW");
        second.Roles.Should().ContainSingle().Which.Should().Be(role2.Id.ToString());

        _permissionServiceMock.VerifyAll();
    }
    #endregion

    #region Delete
    [TestMethod]
    public void DeletePermission_ShouldRemove_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _permissionServiceMock
            .Setup(s => s.Remove(id));

        _permissionController.DeletePermission(id.ToString());

        _permissionServiceMock.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    public void UpdatePermission_ValidInput_ShouldCallServiceUpdate()
    {
        var id = Guid.NewGuid();
        var role1 = Guid.NewGuid().ToString();
        var role2 = Guid.NewGuid().ToString();

        var request = new CreatePermissionRequest
        {
            Description = "Manage users",
            Key = "USERS_MANAGE",
            RolesIds = [role1, role2]
        };

        var expectedArgs = request.ToArgs();

        _permissionServiceMock
            .Setup(s => s.Update(
                id,
                It.Is<PermissionArgs>(a =>
                    a.Description == expectedArgs.Description &&
                    a.Key == expectedArgs.Key &&
                    a.Roles.Count == expectedArgs.Roles.Count &&
                    a.Roles.All(expectedArgs.Roles.Contains))));

        _permissionController.UpdatePermission(request, id.ToString());

        _permissionServiceMock.VerifyAll();
    }
    #endregion
}
