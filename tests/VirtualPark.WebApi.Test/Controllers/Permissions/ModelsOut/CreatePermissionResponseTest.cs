using FluentAssertions;
using VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Permissions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("CreatePermissionResponse")]
public class CreatePermissionResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new CreatePermissionResponse(id);
        response.Id.Should().Be(id);
    }
    #endregion
}
