using FluentAssertions;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("LogInSessionResponse")]
public class LogInSessionResponseTest
{
    #region Token
    [TestMethod]
    [TestCategory("Validation")]
    public void Token_Getter_ReturnsAssignedValue()
    {
        var token = Guid.NewGuid().ToString();
        var response = new LogInSessionResponse(token);
        response.Token.Should().Be(token);
    }
    #endregion

    #region VisitorId
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithVisitorAndRole_ShouldAssignAllProperties()
    {
        var id = Guid.NewGuid().ToString();
        var visitorId = Guid.NewGuid().ToString();
        var visitRegistrationId = Guid.NewGuid().ToString();

        var response = new GetUserLoggedSessionResponse(id, visitorId, visitRegistrationId);

        response.Id.Should().Be(id);
        response.VisitorId.Should().Be(visitorId);
        response.VisitRegistrationId.Should().Be(role);
    }
    #endregion
}
