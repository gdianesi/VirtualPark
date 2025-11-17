using FluentAssertions;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.VisitorsProfile.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetVisitorProfileResponse")]
public class GetVisitorProfileResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30", "Visitor", "10", Guid.NewGuid().ToString(), "30");
        response.Id.Should().Be(id);
    }
    #endregion

    #region DateOfBirth
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30", "Visitor", "10", Guid.NewGuid().ToString(), "30");
        response.DateOfBirth.Should().Be("2002-07-30");
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30", "Visitor", "10", Guid.NewGuid().ToString(), "30");
        response.Membership.Should().Be("Visitor");
    }
    #endregion

    #region Score
    [TestMethod]
    [TestCategory("Validation")]
    public void Score_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30", "Visitor", "10", Guid.NewGuid().ToString(), "30");
        response.Score.Should().Be("10");
    }
    #endregion

    #region NfcId
    [TestMethod]
    [TestCategory("Validation")]
    public void NfcId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var nfcId = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30", "Visitor", "10", nfcId, "30");
        response.NfcId.Should().Be(nfcId);
    }
    #endregion
    #region PointsAvailable
    [TestMethod]
    [TestCategory("Validation")]
    public void PointsAvailable_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var nfcId = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30", "Visitor", "10", nfcId, "30");
        response.PointsAvailable.Should().Be("30");
    }
    #endregion
}
