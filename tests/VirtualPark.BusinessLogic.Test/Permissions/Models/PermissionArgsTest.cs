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

    #region Description
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    [DataRow("")]
    [DataRow(null)]
    public void Constructor_WhenDescriptionIsNullOrEmpty_ShouldThrow(string? description)
    {
        FluentActions
            .Invoking(() => new PermissionArgs(description!, "event.create", [Guid.NewGuid()]))
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
            .Invoking(() => new PermissionArgs("Can create users", description!, [Guid.NewGuid()]))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    [TestMethod]
    [DataRow(null, DisplayName = "Null list")]
    [DataRow("empty", DisplayName = "Empty list")]
    [TestCategory("Validation")]
    public void Constructor_WhenRolesIdsAreNullOrEmpty_ShouldThrowArgumentException(string caseType)
    {
        List<Guid>? ids = caseType switch
        {
            "empty" => [],
            _ => null
        };

        FluentActions.Invoking(() => new PermissionArgs("Can create users", "create-user", ids!))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Roles list cannot be null or empty");
    }
}
