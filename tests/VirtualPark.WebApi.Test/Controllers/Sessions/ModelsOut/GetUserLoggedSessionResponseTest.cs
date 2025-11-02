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
        List<string> roleNames = ["Administrator"];

        var response = new GetUserLoggedSessionResponse(id, visitorId, roleNames);

        response.Id.Should().Be(id);
        response.VisitorId.Should().Be(visitorId);
        response.Roles.Should().BeEquivalentTo(roleNames);
    }
}
