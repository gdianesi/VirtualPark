using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.VisitorsProfile.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetVisitorProfileResponse")]
public class GetVisitorProfileResponseTest
{
    private static VisitorProfile Build(
        Guid? id = null,
        DateOnly? dob = null,
        Membership? membership = null,
        int? score = null,
        Guid? nfcId = null,
        int? points = null)
    {
        return new VisitorProfile
        {
            Id = id ?? Guid.NewGuid(),
            DateOfBirth = dob ?? new DateOnly(2000, 01, 01),
            Membership = membership ?? Membership.Standard,
            Score = score ?? 10,
            NfcId = nfcId ?? Guid.NewGuid(),
            PointsAvailable = points ?? 50
        };
    }

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var vp = Build(id: id);

        var response = new GetVisitorProfileResponse(vp);

        response.Id.Should().Be(id.ToString());
    }
    #endregion

    #region DateOfBirth
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var date = new DateOnly(2002, 07, 30);
        var vp = Build(dob: date);

        var response = new GetVisitorProfileResponse(vp);

        response.DateOfBirth.Should().Be("2002-07-30");
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var vp = Build(membership: Membership.VIP);

        var response = new GetVisitorProfileResponse(vp);

        response.Membership.Should().Be("VIP");
    }
    #endregion

    #region Score
    [TestMethod]
    [TestCategory("Validation")]
    public void Score_Getter_ReturnsAssignedValue()
    {
        var vp = Build(score: 99);

        var response = new GetVisitorProfileResponse(vp);

        response.Score.Should().Be("99");
    }
    #endregion

    #region NfcId
    [TestMethod]
    [TestCategory("Validation")]
    public void NfcId_Getter_ReturnsAssignedValue()
    {
        var nfc = Guid.NewGuid();
        var vp = Build(nfcId: nfc);

        var response = new GetVisitorProfileResponse(vp);

        response.NfcId.Should().Be(nfc.ToString());
    }
    #endregion

    #region PointsAvailable
    [TestMethod]
    [TestCategory("Validation")]
    public void PointsAvailable_Getter_ReturnsAssignedValue()
    {
        var vp = Build(points: 300);

        var response = new GetVisitorProfileResponse(vp);

        response.PointsAvailable.Should().Be("300");
    }
    #endregion
}
