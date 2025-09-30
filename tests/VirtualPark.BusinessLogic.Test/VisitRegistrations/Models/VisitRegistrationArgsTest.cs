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

        var args = new VisitRegistrationArgs(attractions, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

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

        var act = () => new VisitRegistrationArgs(attractions, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        act.Should().Throw<FormatException>()
            .Where(ex => ex.Message.Contains(invalid));
    }
    #endregion
    #endregion

    #region VisitorProfileId
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_ShouldParseValidGuidString()
    {
        var vpId = Guid.NewGuid();
        var attractions = new List<string> { Guid.NewGuid().ToString() };

        var args = new VisitRegistrationArgs(attractions, vpId.ToString(), Guid.NewGuid().ToString());

        args.VisitorProfileId.Should().Be(vpId);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_ShouldThrow_WhenGuidStringIsInvalid()
    {
        var invalidId = "not-a-guid";
        var attractions = new List<string> { Guid.NewGuid().ToString() };

        var act = () => new VisitRegistrationArgs(attractions, invalidId, Guid.NewGuid().ToString());

        act.Should().Throw<FormatException>()
            .Where(ex => ex.Message.Contains(invalidId));
    }
    #endregion
    #endregion

    #region TicketId
    [TestMethod]
    [TestCategory("Validation")]
    public void TicketId_fail()
    {
        var vpId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        var attractions = new List<string> { Guid.NewGuid().ToString() };

        var args = new VisitRegistrationArgs(attractions, vpId.ToString(), ticketId.ToString());

        args.TicketId.Should().Be(ticketId);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void TicketId_ShouldThrow_WhenGuidStringIsInvalid()
    {
        var invalidId = "not-a-guid";
        var attractions = new List<string> { Guid.NewGuid().ToString() };

        var act = () => new VisitRegistrationArgs(attractions, Guid.NewGuid().ToString(), invalidId);

        act.Should().Throw<FormatException>()
            .Where(ex => ex.Message.Contains(invalidId));
    }
    #endregion
}
