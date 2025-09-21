using FluentAssertions;
using VirtualPark.BusinessLogic.Permissions.Entity;

namespace VirtualPark.BusinessLogic.Test.Permissions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Permission")]
public class PermissionTest
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

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Description_SetterGetter_ShouldReturnAssignedValue()
    {
        var permission = new Permission { Description = "Can create tickets" };
        permission.Description.Should().Be("Can create tickets");
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Key_SetterGetter_ShouldReturnAssignedValue()
    {
        var permission = new Permission { Key = "CREATE_TICKET" };
        permission.Key.Should().Be("CREATE_TICKET");
    }
}
