using FluentAssertions;
using VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Permissions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetPermissionResponse")]
public class GetPermissionResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetPermissionResponse(id, "description", "key", [Guid.NewGuid().ToString()]);
        response.Id.Should().Be(id);
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetPermissionResponse(id, "description", "key", [Guid.NewGuid().ToString()]);
        response.Description.Should().Be("description");
    }
    #endregion

    #region Key
    [TestMethod]
    [TestCategory("Validation")]
    public void Key_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetPermissionResponse(id, "description", "key", [Guid.NewGuid().ToString()]);
        response.Key.Should().Be("key");
    }
    #endregion

    #region Roles
    [TestMethod]
    [TestCategory("Validation")]
    public void Role_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var guid = Guid.NewGuid().ToString();
        var response = new GetPermissionResponse(id, "description", "key", [guid]);
        response.Roles.Should().Contain([guid]);
    }
    #endregion
}
