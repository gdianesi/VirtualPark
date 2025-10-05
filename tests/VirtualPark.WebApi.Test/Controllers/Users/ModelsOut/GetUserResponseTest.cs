using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserResponse")]
public class GetUserResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id, "pepe");
        response.Id.Should().Be(id);
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id, "pepe");
        response.Name.Should().Be("pepe");
    }
}
