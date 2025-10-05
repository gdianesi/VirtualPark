namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserResponse")]
public class GetUserResponse
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
