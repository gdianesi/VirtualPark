using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateSessionRequest")]
public class CreateSessionRequestTest
{
    #region UserId
    [TestMethod]
    [TestCategory("Validation")]
    public void UserId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var createSessionRequest = new CreateSessionRequest { UserId = id };
        createSessionRequest.UserId.Should().Be(id);
    }
    #endregion
}
