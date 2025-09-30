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
    #region AttractionId
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionsId_ShouldParseAllGuids_InOrder()
    {
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var attractions = new List<string> { g1.ToString(), g2.ToString() };

        var args = new VisitRegistrationArgs(attractions);

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

        var act = () => new VisitRegistrationArgs(attractions);

        act.Should().Throw<FormatException>()
            .Where(ex => ex.Message.Contains(invalid));
    }
    #endregion
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_success()
    {
        var vpId = Guid.NewGuid();
        var attractions = new List<string> { Guid.NewGuid().ToString() };

        var args = new VisitRegistrationArgs(attractions, vpId.ToString());

        args.VisitorProfileId.Should().Be(vpId);
    }
}
