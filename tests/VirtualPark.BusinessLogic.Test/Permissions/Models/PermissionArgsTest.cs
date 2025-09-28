namespace VirtualPark.BusinessLogic.Test.Permissions.Models;

[TestClass]
public sealed class PermissionArgsTest
{
    [TestMethod]
    public void Constructor_WhenValidInputs_ShouldCreatePermissionArgs()
    {
        var roles = new List<Guid> { Guid.NewGuid() };

        var args = new PermissionArgs("Can create events", "event.create", roles);

        args.Description.Should().Be("Can create events");
        args.Key.Should().Be("event.create");
        args.Roles.Should().HaveCount(1);
    }
}

