using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsIn;

[TestClass]
[TestCategory("ValidateEntryByNfcRequest")]
public class ValidateEntryByNfcRequestTest
{
    #region VisitorId
    [TestMethod]
    public void ValidateEntryByNfcRequest_VisitorIdProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var visitorId = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee";

        var request = new ValidateEntryByNfcRequest { VisitorId = visitorId };

        request.VisitorId.Should().Be(visitorId);
    }
    #endregion
}
