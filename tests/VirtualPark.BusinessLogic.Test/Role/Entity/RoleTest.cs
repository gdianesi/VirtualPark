using FluentAssertions;

namespace VirtualPark.BusinessLogic.Test.Role.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Role")]
public sealed class RoleTest
{
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenRoleIsCreated_ShouldAssignId()
    {
        // Act
        var role = new Roles.Entity.Role();

        // Assert
        role.Id.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Name_SetterGetter_ShouldReturnAssignedValue()
    {
        var role = new Roles.Entity.Role { Name = "Admin" };
        role.Name.Should().Be("Admin");
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Description_SetterGetter_ShouldReturnAssignedValue()
    {
        var role = new Role { Description = "Administrator role with full access" };
        role.Description.Should().Be("Administrator role with full access");
    }
}
