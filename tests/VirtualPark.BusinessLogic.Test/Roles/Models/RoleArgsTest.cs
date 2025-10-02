using FluentAssertions;
using VirtualPark.BusinessLogic.Roles.Models;

namespace VirtualPark.BusinessLogic.Test.Roles.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("RolesArgs")]
public sealed class RoleArgsTest
{
    #region Name
    [TestMethod]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var roleArgs = new RoleArgs("Visitor", "Description", new List<string>(), new List<string>());
        roleArgs.Name.Should().Be("Visitor");
    }
    #endregion

    #region Description
    [TestMethod]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var roleArgs = new RoleArgs("Visitor", "Description", new List<string>(), new List<string>());
        roleArgs.Description.Should().Be("Description");
    }
    #endregion

    #region PermissionIds
    [TestMethod]
    public void PermissionIds_Getter_ReturnsParsedGuids()
    {
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var permissions = new List<string> { g1.ToString(), g2.ToString() };

        var roleArgs = new RoleArgs("Visitor", "Description", permissions, new List<string>());

        roleArgs.PermissionIds.Should().HaveCount(2);
        roleArgs.PermissionIds.Should().ContainInOrder(g1, g2);
    }

    [TestMethod]
    public void PermissionIds_WhenInvalidGuid_ShouldThrowFormatException()
    {
        var g3 = Guid.NewGuid();
        var users = new List<string> { g3.ToString() };
        var invalidPermissions = new List<string>() { "not-a-guid" };

        FluentAssertions.FluentActions
            .Invoking(() => new RoleArgs("Visitor", "Description", invalidPermissions, users))
            .Should().Throw<FormatException>()
            .WithMessage("The value 'not-a-guid' is not a valid Guid.");
    }

    #endregion

    #region UsersIds
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void UsersId_getter_ReturnsAssignedValue()
    {
        var g3 = Guid.NewGuid();
        var users = new List<string> { g3.ToString() };

        var roleArgs = new RoleArgs("Visitor", "Description", new List<string>(), users);

        roleArgs.UsersIds.Should().HaveCount(1);
        roleArgs.UsersIds.Should().Contain([g3]);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void UsersIds_WithInvalidUserId_ThrowsFormatException()
    {
        var users = new List<string> { "guid" };

        var act = () => new RoleArgs("Visitor", "Description", new List<string>(), users);

        act.Should()
            .Throw<FormatException>();
    }
    #endregion
    #endregion
}
