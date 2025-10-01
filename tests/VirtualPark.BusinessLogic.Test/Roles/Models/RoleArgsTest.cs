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
        var roleArgs = new RoleArgs("Visitor");
        roleArgs.Name.Should().Be("Visitor");
    }
    #endregion
}
