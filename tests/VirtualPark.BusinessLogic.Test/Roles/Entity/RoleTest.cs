using FluentAssertions;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Test.Roles.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Role")]
public sealed class RoleTest
{
    #region Id

    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenRoleIsCreated_ShouldAssignId()
    {
        // Act
        var role = new Role();

        // Assert
        role.Id.Should().NotBe(Guid.Empty);
    }

    #endregion

    #region Name

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Name_SetterGetter_ShouldReturnAssignedValue()
    {
        var role = new Role { Name = "Admin" };
        role.Name.Should().Be("Admin");
    }

    #endregion

    #region Description

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Description_SetterGetter_ShouldReturnAssignedValue()
    {
        var role = new Role { Description = "Administrator role with full access" };
        role.Description.Should().Be("Administrator role with full access");
    }

    #endregion

    #region Role

    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenRoleIsCreated_ShouldInitializePermissionsList()
    {
        var role = new Role();

        role.Permissions.Should().NotBeNull();
        role.Permissions.Should().BeEmpty();
    }

    #endregion

    #region Users
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void User_Getter_ReturnsAssignedValue()
    {
        var users = new List<User> { new User { Name = "Admin" } };
        var role = new Role { Users = users };
        role.Users.Should().BeEquivalentTo(users);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void User_Setter_ReturnsAssignedValue()
    {
        var users = new List<User> { new User { Name = "Admin" } };
        var role = new Role();
        role.Users = users;
        role.Users.Should().BeEquivalentTo(users);
    }
    #endregion
    #endregion
}
