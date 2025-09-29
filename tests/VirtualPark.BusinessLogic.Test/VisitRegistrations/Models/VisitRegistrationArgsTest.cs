using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;

namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("VisitRegistrationArgs")]
public class VisitRegistrationArgsTest
{
    #region VisitorProfile
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_ShouldBeAssigned_FromConstructor()
    {
        var vp = new VisitorProfileArgs("2002-07-30", "Standard", "85");
        var g = Guid.NewGuid();
        var attractions = new List<string> { g.ToString() };

        var args = new VisitRegistrationArgs(vp, attractions);

        args.VisitorProfile.Should().NotBeNull();
        args.VisitorProfile.Should().BeSameAs(vp);
        args.VisitorProfile.DateOfBirth.Should().Be(new DateOnly(2002, 7, 30));
        args.VisitorProfile.Membership.Should().Be(Membership.Standard);
        args.VisitorProfile.Score.Should().Be(85);
    }
    #endregion

    #region AttractionId
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionsId_ShouldParseAllGuids_InOrder()
    {
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var attractions = new List<string> { g1.ToString(), g2.ToString() };

        var vp = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        var args = new VisitRegistrationArgs(vp, attractions);

        args.AttractionsId.Should().NotBeNull();
        args.AttractionsId.Should().HaveCount(2);
        args.AttractionsId.Should().ContainInOrder(new[] { g1, g2 });
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionsId_ShouldThrow_WhenAnyValueIsNotAGuid()
    {
        var invalid = "not-a-guid";
        var attractions = new List<string> { invalid };

        var vp = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        var act = () => new VisitRegistrationArgs(vp, attractions);

        act.Should().Throw<FormatException>()
            .Where(ex => ex.Message.Contains(invalid));
    }
    #endregion
    #endregion
}
