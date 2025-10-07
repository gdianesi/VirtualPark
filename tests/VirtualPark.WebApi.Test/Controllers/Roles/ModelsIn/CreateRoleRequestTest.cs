using FluentAssertions;
using VirtualPark.WebApi.Controllers.Roles.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Roles.ModelsIn;

[TestClass]
public class CreateRoleRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRoleRequest { Name = "Admin" };
        request.Name.Should().Be("Admin");
    }
    #endregion
    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRoleRequest { Description = "Full access to system" };
        request.Description.Should().Be("Full access to system");
    }
    #endregion
    #region Permissions
    [TestMethod]
    [TestCategory("Validation")]
    public void Permissions_Getter_ReturnsAssignedValue()
    {
        var perms = new List<string> { "users.read", "users.write" };
        var request = new CreateRoleRequest { Permissions = perms };

        request.Permissions.Should().Contain(new[] { "users.read", "users.write" });
    }
    #endregion
}
