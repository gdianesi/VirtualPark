using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsIn;

[TestClass]
[TestCategory("ValidateEntryByQrRequest")]
public class ValidateEntryByQrRequestTest
{
    #region QrId
    [TestMethod]
    public void ValidateEntryByQrRequest_QrIdProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var qr = "11111111-2222-3333-4444-555555555555";

        var request = new ValidateEntryByQrRequest { QrId = qr };

        request.QrId.Should().Be(qr);
    }
    #endregion
}
