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
        var id = Guid.NewGuid().ToString();
        var response = new ReportAttractionsResponse("Roller");
        response.Name.Should().Be("Roller");
    }
    #endregion
}
