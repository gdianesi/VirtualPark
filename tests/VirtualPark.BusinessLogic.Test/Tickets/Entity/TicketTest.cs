using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;

namespace VirtualPark.BusinessLogic.Test.Tickets.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Ticket")]
public sealed class TicketTest
{
    #region ID
    [TestMethod]
    public void WhenTicketIsCreated_IdIsAssigned()
    {
        var ticket = new Ticket();
        ticket.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
    #region Date
    [TestMethod]
    public void Date_GetSet_Works()
    {
        DateTime d = DateTime.Now;
        var ticket = new Ticket { Date = d };
        ticket.Date.Should().Be(d);
    }
    #endregion
    #region Type
    [TestMethod]
    public void Type_GetSet_Works()
    {
        var ticket = new Ticket { Type = EntranceType.General };
        ticket.Type.Should().Be(EntranceType.General);
    }
    #endregion
    #region Event
    [TestMethod]
    public void EventId_GetSet_Works()
    {
        var ticket = new Ticket { EventId = Guid.NewGuid() };
        ticket.EventId.Should().Be(ticket.EventId);
    }
    #endregion

    #region QrId
    [TestMethod]
    public void WhenTicketIsCreated_QrIdIsAssigned()
    {
        var ticket = new Ticket();
        ticket.QrId.Should().NotBe(Guid.Empty);
    }
    #endregion
}
