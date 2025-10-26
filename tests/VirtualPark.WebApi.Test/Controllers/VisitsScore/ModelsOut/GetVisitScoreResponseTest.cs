using FluentAssertions;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.VisitsScore.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetVisitScoreResponse")]
public class GetVisitScoreResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var regId = Guid.NewGuid().ToString();

        var resp = new GetVisitScoreResponse(
            id,
            "Attraction");

        resp.Id.Should().Be(id);
    }
    #endregion

    #region Origin
    [TestMethod]
    [TestCategory("Validation")]
    public void Origin_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var regId = Guid.NewGuid().ToString();

        var resp = new GetVisitScoreResponse(
            id,
            "Attraction");

        resp.Origin.Should().Be("Attraction");
    }
    #endregion
}
