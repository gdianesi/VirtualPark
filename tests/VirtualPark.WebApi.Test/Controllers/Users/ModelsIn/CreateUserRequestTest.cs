namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateUserRequest")]
public class CreateUserRequestTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter()
    {
        var createUserRequest = new CreateUserRequest { Name = "Pepe" };
        createUserRequest.Name.Should().Be("Pepe");
    }
}
