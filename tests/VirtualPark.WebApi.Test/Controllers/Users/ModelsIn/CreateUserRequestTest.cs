using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateUserRequest")]
public class CreateUserRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var createUserRequest = new CreateUserRequest { Name = "Pepe" };
        createUserRequest.Name.Should().Be("Pepe");
    }
    #endregion
}
