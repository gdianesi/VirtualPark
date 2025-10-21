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
        var roles = new List<string> { Guid.NewGuid().ToString() };

        var args = new PermissionArgs("Can create events", "event.create", roles);

        args.Description.Should().Be("Can create events");
        args.Key.Should().Be("event.create");
        args.Roles.Should().HaveCount(1);
    }

    #endregion

    #region Description
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    [DataRow("")]
    [DataRow(null)]
    public void Constructor_WhenDescriptionIsNullOrEmpty_ShouldThrow(string? description)
    {
        FluentActions
            .Invoking(() => new PermissionArgs(description!, "event.create", [Guid.NewGuid().ToString()]))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    #region Key
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    [DataRow("")]
    [DataRow(null)]
    public void Constructor_WhenKeyIsNullOrEmpty_ShouldThrow(string? description)
    {
        FluentActions
            .Invoking(() => new PermissionArgs("Can create users", description!, [Guid.NewGuid().ToString()]))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    #region Roles
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void RolesIds_WithInvalidRoleId_ThrowsFormatException()
    {
        var roles = new List<string> { "guid" };

        var act = () => new PermissionArgs("Can create users", "create-user", roles);

        act.Should()
            .Throw<FormatException>();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_EventsIdsContainEmptyGuid_ShouldThrowArgumentException()
    {
        var invalidIds = new List<string> { Guid.NewGuid().ToString(), "guid" };

        FluentActions.Invoking(() => new PermissionArgs("Can create users", "create-user", invalidIds))
            .Should()
            .Throw<FormatException>()
            .WithMessage("The value 'guid' is not a valid GUID.");
    }
    #endregion
    #endregion
}
