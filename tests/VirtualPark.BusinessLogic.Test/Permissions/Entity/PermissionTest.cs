using FluentAssertions;
using VirtualPark.BusinessLogic.Permissions.Entity;

namespace VirtualPark.BusinessLogic.Test.Permissions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Permission")]
public class PermissionTest
{
    #region ID

    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenPermissionIsCreated_ShouldAssignId()
    {
        var permission = new Permission();

        permission.Id.Should().NotBe(Guid.Empty);
    }

    #endregion

    #region Description

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Description_SetterGetter_ShouldReturnAssignedValue()
    {
        var permission = new Permission { Description = "Can create tickets" };
        permission.Description.Should().Be("Can create tickets");
    }

    #endregion

    #region Key

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Key_SetterGetter_ShouldReturnAssignedValue()
    {
        var permission = new Permission { Key = "CREATE_TICKET" };
        permission.Key.Should().Be("CREATE_TICKET");
    }

    #endregion

    #region Roles

    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenPermissionIsCreated_ShouldInitializeRolesList()
    {
        var permission = new Permission();

        permission.Roles.Should().NotBeNull();
        permission.Roles.Should().BeEmpty();
    }

    #endregion
}
