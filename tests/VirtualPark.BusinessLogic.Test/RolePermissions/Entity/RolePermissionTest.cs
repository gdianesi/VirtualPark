using FluentAssertions;
using VirtualPark.BusinessLogic.RolePermissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.BusinessLogic.Test.RolePermissions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("RolePermissionTest")]
public class RolePermissionTest
{
    #region RoleId
    [TestMethod]
    [TestCategory("Validation")]
    public void RolePermission_GetterRoleId_ReturnsAssignedValue()
    {
        var role = new Role();
        var rolePermission = new RolePermission { RoleId = role.Id };
        rolePermission.RoleId.Should().Be(role.Id);
    }
    #endregion
}
