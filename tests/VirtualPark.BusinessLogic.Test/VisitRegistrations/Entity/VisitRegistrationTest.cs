using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitRegistration")]
public sealed class VisitRegistrationTest
{
    #region Id
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldAssignId()
    {
        var visit = new VisitRegistration();
        visit.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldInitializeDate()
    {
        var visit = new VisitRegistration();
        visit.Date.Should().NotBe(default);
    }
    #endregion

    #region Attractions
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldInitializeAttractionsList()
    {
        var visit = new VisitRegistration();
        visit.Attractions.Should().NotBeNull();
        visit.Attractions.Should().BeEmpty();
    }
    #endregion

    #region Visitor
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Visitor_WhenAssigned_ShouldBeStored()
    {
        var visitor = new VisitorProfile();

        var visit = new VisitRegistration { Visitor = visitor };

        visit.Visitor.Should().Be(visitor);
    }
    #endregion

    #region Ticket
    [TestMethod]
    [TestCategory("Getter")]
    public void Ticket_Getter_ShouldReturnAssignedInstance()
    {
        var ticket = new Ticket();
        var visit = new VisitRegistration { Ticket = ticket };

        visit.Ticket.Should().NotBeNull();
        visit.Ticket.Should().BeSameAs(ticket);
    }

    [TestMethod]
    [TestCategory("Setter")]
    public void Ticket_ShouldBeSettable()
    {
        var ticket = new Ticket();
        var visit = new VisitRegistration();

        visit.Ticket = ticket;

        visit.Ticket.Should().NotBeNull();
        visit.Ticket.Should().BeSameAs(ticket);
    }
    #endregion
}
