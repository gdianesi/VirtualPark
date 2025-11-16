using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("VisitorInAttractionResponse")]
public class VisitorInAttractionResponseTest
{
    #region VisitorProfileId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_Getter_ReturnsAssignedValue()
    {
        var visitorProfileId = Guid.NewGuid();
        var response = new VisitorInAttractionResponse
        {
            VisitorProfileId = visitorProfileId
        };

        response.VisitorProfileId.Should().Be(visitorProfileId);
    }
    #endregion

    #region UserId
    [TestMethod]
    [TestCategory("Validation")]
    public void UserId_Getter_ReturnsAssignedValue()
    {
        var userId = Guid.NewGuid();
        var response = new VisitorInAttractionResponse
        {
            UserId = userId
        };

        response.UserId.Should().Be(userId);
    }
    #endregion
}
