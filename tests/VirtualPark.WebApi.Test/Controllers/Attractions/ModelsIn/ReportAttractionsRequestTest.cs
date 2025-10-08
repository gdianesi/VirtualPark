using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsIn;

[TestClass]
[TestCategory("CreateAttractionRequest")]
public class ReportAttractionsRequestTest
{
    #region From
    [TestMethod]
    public void ReportAttractionRequest_FromProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new ReportAttractionsRequest { From = "2025-04-23" };
        attraction.From.Should().Be("2025-04-23");
    }
    #endregion

    #region To
    [TestMethod]
    public void ReportAttractionRequest_ToProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new ReportAttractionsRequest { To = "2025-05-23" };
        attraction.To.Should().Be("2025-05-23");
    }
    #endregion
}
