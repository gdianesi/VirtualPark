using FluentAssertions;
using VirtualPark.BusinessLogic.Visitors.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Test.VisitorsProfile.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitorProfile")]
public class VisitorProfileTest
{
    #region id

    [TestMethod]
    [TestCategory("Validation")]
    public void Visitor_WhenCreated_ShouldHaveNonEmptyId()
    {
        var visitorProfile = new VisitorProfile();
        visitorProfile.Id.Should().NotBe(System.Guid.Empty);
    }

    #endregion

    #region DateOfBirth

    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var visitorProfile = new VisitorProfile { DateOfBirth = new DateOnly(2002, 07, 30) };
        visitorProfile.DateOfBirth.Should().Be(new DateOnly(2002, 07, 30));
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Setter_ReturnsAssignedValue()
    {
        var visitorProfile = new VisitorProfile();
        visitorProfile.DateOfBirth = new DateOnly(2002, 07, 30);
        visitorProfile.DateOfBirth.Should().Be(new DateOnly(2002, 07, 30));
    }

    #endregion

    #region Membership

    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var visitorProfile = new VisitorProfile { Membership = Membership.Standard };
        visitorProfile.Membership.Should().Be(Membership.Standard);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Setter_ReturnsAssignedValue()
    {
        var visitorProfile = new VisitorProfile();
        visitorProfile.Membership = Membership.Standard;
        visitorProfile.Membership.Should().Be(Membership.Standard);
    }

    #endregion

    #region Score

    [TestMethod]
    [TestCategory("Constructor")]
    public void Score_WhenVisitorIsCreated_ShouldBeZeroByDefault()
    {
        var visitorProfile = new VisitorProfile();

        visitorProfile.Score.Should().Be(0);
    }

    #endregion

    #region NfcId
    [TestMethod]
    [TestCategory("Constructor")]
    public void NfcId_WhenVisitorProfileIsCreated_ShouldNotBeEmpty()
    {
        var visitorProfile = new VisitorProfile();

        visitorProfile.NfcId.Should().NotBe(Guid.Empty);
    }
    #endregion
}
