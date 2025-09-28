using FluentAssertions;
using VirtualPark.BusinessLogic.Permissions.Models;

namespace VirtualPark.BusinessLogic.Test.Permissions.Models;

[TestClass]
[TestCategory("Args")]
[TestCategory("Permission")]
public sealed class PermissionArgsTest
{
    #region Constructor

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Constructor_WhenValidInputs_ShouldCreatePermissionArgs()
    {
        var roles = new List<Guid> { Guid.NewGuid() };

        var args = new PermissionArgs("Can create events", "event.create", roles);

        args.Description.Should().Be("Can create events");
        args.Key.Should().Be("event.create");
        args.Roles.Should().HaveCount(1);
    }

    #endregion

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Constructor_WhenDescriptionIsNullOrEmpty_ShouldThrow(string? description)
    {
        FluentActions
            .Invoking(() => new PermissionArgs(description!, "event.create", new List<Guid> { Guid.NewGuid() }))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }
}
