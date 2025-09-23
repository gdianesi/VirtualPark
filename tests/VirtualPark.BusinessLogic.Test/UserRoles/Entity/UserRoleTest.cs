using FluentAssertions;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.UserRoles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Test.UserRoles.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("User")]
public class UserRoleTest
{
    #region  UserId
    [TestMethod]
    [TestCategory("Validation")]
    public void UserRole_GetterUserId_ReturnsAssignedValue()
    {
        User user = new User();
        var userRole = new UserRole { UserId = user.Id };
        userRole.UserId.Should().Be(user.Id);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void UserRole_setterUserId_ReturnsAssignedValue()
    {
        User user = new User();
        var userRole = new UserRole();
        userRole.UserId = user.Id;
        userRole.UserId.Should().Be(user.Id);
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void UserRole_GetterRoleId_ReturnsAssignedValue()
    {
        Role role = new Role();
        var userRole = new UserRole { RoleId = role.Id };
        userRole.RoleId.Should().Be(role.Id);
    }
}
