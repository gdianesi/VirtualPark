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
        var role = new Role();

        // Assert
        role.Id.Should().NotBe(Guid.Empty);
    }
}
