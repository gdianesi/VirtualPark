using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Tickets.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetTicketResponse")]
public sealed class GetTicketResponseTest
{
    private static Ticket BuildTicket(
        Guid? id = null,
        EntranceType? type = null,
        DateTime? date = null,
        Guid? qrId = null,
        Guid? eventId = null,
        Guid? visitorId = null)
    {
        return new Ticket
        {
            Id = id ?? Guid.NewGuid(),
            Type = type ?? EntranceType.General,
            Date = date ?? new DateTime(2025, 12, 1),
            QrId = qrId ?? Guid.NewGuid(),
            EventId = eventId,
            VisitorProfileId = visitorId ?? Guid.NewGuid(),

            Event = null!,
            Visitor = null!
        };
    }

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var ticket = BuildTicket(id: id);

        var response = new GetTicketResponse(ticket);

        response.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var ticket = BuildTicket(type: EntranceType.Event);

        var response = new GetTicketResponse(ticket);

        response.Type.Should().Be("Event");
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var date = new DateTime(2025, 12, 1);
        var ticket = BuildTicket(date: date);

        var response = new GetTicketResponse(ticket);

        response.Date.Should().Be("2025-12-01");
    }
    #endregion

    #region QrId
    [TestMethod]
    [TestCategory("Validation")]
    public void QrId_Getter_ReturnsAssignedValue()
    {
        var qr = Guid.NewGuid();
        var ticket = BuildTicket(qrId: qr);

        var response = new GetTicketResponse(ticket);

        response.QrId.Should().Be(qr.ToString());
    }
    #endregion

    #region EventId
    [TestMethod]
    [TestCategory("Validation")]
    public void EventId_Getter_ReturnsAssignedValue()
    {
        var ev = Guid.NewGuid();
        var ticket = BuildTicket(eventId: ev);

        var response = new GetTicketResponse(ticket);

        response.EventId.Should().Be(ev.ToString());
    }
    #endregion

    #region VisitorId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorId_Getter_ReturnsAssignedValue()
    {
        var visitor = Guid.NewGuid();
        var ticket = BuildTicket(visitorId: visitor);

        var response = new GetTicketResponse(ticket);

        response.VisitorId.Should().Be(visitor.ToString());
    }
    #endregion
}
