using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.ViaAccess.Entity;
using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.Test.ViaAccess.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("QrTest")]
public sealed class QrTest
{
    #region Ticket
    [TestMethod]
    [TestCategory("Behaviour")]
    public void IdentifyTicketVisitor_WhenCreatedWithTicket_ShouldReturnSameTicketVisitor()
    {
        var visitor = new Visitor();
        var ticket = new Ticket { Visitor = visitor };

        var qr = new Qr(ticket);

        qr.IdentifyVisitor().Should().Be(visitor);
    }
    #endregion
}
