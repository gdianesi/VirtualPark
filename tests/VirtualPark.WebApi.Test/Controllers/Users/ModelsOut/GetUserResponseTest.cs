using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserResponse")]
public class GetUserResponseTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id);
        response.Id.Should().Be(id);
    }
}
