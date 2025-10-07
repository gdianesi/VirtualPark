using FluentAssertions;
using VirtualPark.WebApi.Controllers.Roles.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Roles.ModelsOut;

[TestClass]

public class CreateRoleResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new CreateRoleResponse(id);
        response.Id.Should().Be(id);
    }
    #endregion
}
