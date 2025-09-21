namespace VirtualPark.BusinessLogic.Test.Permission.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Permission")]
public sealed class PermissionTest
{
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenPermissionIsCreated_ShouldAssignId()
    {
        // Act
        var permission = new Permission();

        // Assert
        permission.Id.Should().NotBe(Guid.Empty);
    }
}
