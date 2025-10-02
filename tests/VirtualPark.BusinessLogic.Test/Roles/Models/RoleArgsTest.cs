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
        var roleArgs = new RoleArgs("Visitor", "Description", []);
        roleArgs.Name.Should().Be("Visitor");
    }
    #endregion

    #region Description
    [TestMethod]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var roleArgs = new RoleArgs("Visitor", "Description", []);
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

        var roleArgs = new RoleArgs("Visitor", "Description", permissions);

        roleArgs.PermissionIds.Should().HaveCount(2);
        roleArgs.PermissionIds.Should().ContainInOrder(g1, g2);
    }

    [TestMethod]
    public void PermissionIds_WhenInvalidGuid_ShouldThrowFormatException()
    {
        var invalidPermissions = new List<string>() { "not-a-guid" };

        FluentAssertions.FluentActions
            .Invoking(() => new RoleArgs("Visitor", "Description", invalidPermissions))
            .Should().Throw<FormatException>()
            .WithMessage("The value 'not-a-guid' is not a valid Guid.");
    }

    #endregion
}
