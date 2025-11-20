using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;

namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("VisitorInAttraction")]
public class VisitorInAttractionTest
{
    #region VisitRegistrationId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitRegistrationId_Getter_ReturnsAssignedValue()
    {
        var visitRegistrationId = Guid.NewGuid();

        var visitor = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Standard
        };

        var model = new VisitorInAttraction
        {
            VisitRegistrationId = visitRegistrationId,
            Visitor = visitor
        };

        model.VisitRegistrationId.Should().Be(visitRegistrationId);
    }
    #endregion

    #region Visitor
    [TestMethod]
    [TestCategory("Validation")]
    public void Visitor_Getter_ReturnsAssignedInstance()
    {
        var visitRegistrationId = Guid.NewGuid();

        var visitor = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Premium,
            Score = 150
        };

        var model = new VisitorInAttraction
        {
            VisitRegistrationId = visitRegistrationId,
            Visitor = visitor
        };

        model.Visitor.Should().BeSameAs(visitor);
        model.Visitor.Score.Should().Be(150);
        model.Visitor.Membership.Should().Be(Membership.Premium);
    }
    #endregion

    #region TicketType
    [TestMethod]
    [TestCategory("Validation")]
    public void TicketType_Getter_ReturnsAssignedValue()
    {
        var visitRegistrationId = Guid.NewGuid();

        var visitor = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Standard
        };

        var ticketType = (EntranceType)1;

        var model = new VisitorInAttraction
        {
            VisitRegistrationId = visitRegistrationId,
            Visitor = visitor,
            TicketType = ticketType
        };

        model.TicketType.Should().Be(ticketType);
    }
    #endregion
}
