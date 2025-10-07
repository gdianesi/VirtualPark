using FluentAssertions;
using VirtualPark.WebApi.Controllers.Roles.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Roles.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRoleResponse")]
public class GetRoleResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetRoleResponse(
            id,
            "Admin",
            "Full access",
            [Guid.NewGuid().ToString()],
            [Guid.NewGuid().ToString()]);

        response.Id.Should().Be(id);
    }
    #endregion
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var response = new GetRoleResponse(
            Guid.NewGuid().ToString(),
            "Manager",
            "Manage users and content",
            [Guid.NewGuid().ToString()],
            [Guid.NewGuid().ToString()]);

        response.Name.Should().Be("Manager");
    }
    #endregion
    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var response = new GetRoleResponse(
            Guid.NewGuid().ToString(),
            "Reporter",
            "Read-only access",
            [Guid.NewGuid().ToString()],
            [Guid.NewGuid().ToString()]);

        response.Description.Should().Be("Read-only access");
    }
    #endregion
    #region PermissionIds
    [TestMethod]
    [TestCategory("Validation")]
    public void PermissionIds_Getter_ReturnsAssignedValue()
    {
        var p1 = Guid.NewGuid().ToString();
        var p2 = Guid.NewGuid().ToString();

        var response = new GetRoleResponse(
            Guid.NewGuid().ToString(),
            "Custom",
            "Custom permissions",
            [p1, p2],
            [Guid.NewGuid().ToString()]);

        response.PermissionIds.Should().Contain([p1, p2]);
    }
    #endregion
    #region UsersIds
    [TestMethod]
    [TestCategory("Validation")]
    public void UsersIds_Getter_ReturnsAssignedValue()
    {
        var u1 = Guid.NewGuid().ToString();
        var u2 = Guid.NewGuid().ToString();

        var response = new GetRoleResponse(
            Guid.NewGuid().ToString(),
            "Support",
            "Limited support access",
            [Guid.NewGuid().ToString()],
            [u1, u2]);

        response.UsersIds.Should().Contain([u1, u2]);
    }
    #endregion
}
