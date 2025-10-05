using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("CreateUserResponse")]
public class CreateUserResponseTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter()
    {
        var id = Guid.NewGuid().ToString();
        var response = new CreateUserResponse(id);
        response.Id.Should().Be(id);
    }
}
