using FluentAssertions;
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
    public void UserRole_UserIdshoulBeGettable()
    {
        User user = new User();
        var userRole = new UserRole { UserId = user.Id };
        userRole.UserId.Should().Be(user.Id);
    }
    #endregion
}
