using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsOut;

[TestClass]
[TestCategory("ReportAttractionResponse")]
public class ReportAttractionsResponseTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var response = new ReportAttractionsResponse("Roller", "1");
        response.Name.Should().Be("Roller");
    }
    #endregion

    #region Visits
    [TestMethod]
    [TestCategory("Validation")]
    public void Visits_Getter_ReturnsAssignedValue()
    {
        var response = new ReportAttractionsResponse("Roller", "1");
        response.Visits.Should().Be("1");
    }
    #endregion
}
