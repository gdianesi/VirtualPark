using FluentAssertions;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.RolePermissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.BusinessLogic.Test.RolePermissions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("RolePermissionTest")]
public class RolePermissionTest
{
    #region RoleId
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void RolePermission_GetterRoleId_ReturnsAssignedValue()
    {
        var role = new Role();
        var rolePermission = new RolePermission { RoleId = role.Id };
        rolePermission.RoleId.Should().Be(role.Id);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void RolePermission_SetterRoleId_ReturnsAssignedValue()
    {
        var role = new Role();
        var rolePermission = new RolePermission();
        rolePermission.RoleId = role.Id;
        rolePermission.RoleId.Should().Be(role.Id);
    }
    #endregion
    #endregion

    #region PermissionId
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void RolePermission_GetterPermissionId_ReturnsAssignedValue()
    {
        var permission = new Permission();
        var rolePermission = new RolePermission { PermissionId = permission.Id };
        rolePermission.PermissionId.Should().Be(permission.Id);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void RolePermission_SetterPermissionId_ReturnsAssignedValue()
    {
        var permission = new Permission();
        var rolePermission = new RolePermission();
        rolePermission.PermissionId = permission.Id;
        rolePermission.PermissionId.Should().Be(permission.Id);
    }
    #endregion
    #endregion
}
