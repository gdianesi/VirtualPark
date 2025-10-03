using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.BusinessLogic.Test.RolePermissions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("RolePermission")]
public class RolePermission
{
    [TestMethod]
    [TestCategory("Validation")]
    public void UserRole_GetterUserId_ReturnsAssignedValue()
    {
        var role = new Role();
        var rolePermission = new RolePermission { RoleId = role.Id };
        rolePermission.RoleId.Should().Be(role.Id);
    }
}
