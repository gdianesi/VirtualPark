using FluentAssertions;
using VirtualPark.WebApi.Controllers.Permissions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Permissions.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreatePermissionRequest")]
public class CreatePermissionRequestTest
{
    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var createPermissionRequest = new CreatePermissionRequest { Description = "description" };
        createPermissionRequest.Description.Should().Be("description");
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Key_Getter_ReturnsAssignedValue()
    {
        var createPermissionRequest = new CreatePermissionRequest { Key = "key" };
        createPermissionRequest.Key.Should().Be("key");
    }
}
