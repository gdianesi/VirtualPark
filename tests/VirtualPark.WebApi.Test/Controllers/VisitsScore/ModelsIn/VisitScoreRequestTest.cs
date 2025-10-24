namespace VirtualPark.WebApi.Test.Controllers.VisitsScore.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("VisitScoreRequest")]
public class VisitScoreRequestTest
{
    #region VisitRegistrationId
    [TestMethod]
    [TestCategory("Getter")]
    public void VisitRegistrationId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var req = new VisitScoreRequest { VisitRegistrationId = id };
        req.VisitRegistrationId.Should().Be(id);
    }
    #endregion
}
