using FluentAssertions;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserLoggedSessionResponseTest")]
public class GetUserLoggedSessionResponseTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithVisitorAndRole_ShouldAssignAllProperties()
    {
        var id = Guid.NewGuid().ToString();
        var visitorId = Guid.NewGuid().ToString();

        var response = new GetUserLoggedSessionResponse(id, visitorId);

        response.Id.Should().Be(id);
        response.VisitorId.Should().Be(visitorId);
    }
}
