using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.ViaAccess.Entity;

namespace VirtualPark.BusinessLogic.Test.ViaAccess.Entity;

[TestCategory("Entity")]
[TestCategory("QrTest")]
public sealed class QrTest
{
    [TestMethod]
    [TestCategory("Behaviour")]
    public void IdentifyTicket_WhenCreatedWithTicket_ShouldReturnSameTicket()
    {
        var ticket = new Ticket();

        var qr = new Qr(ticket);

        qr.IdentifyVisitor().Should().Be(ticket);
    }
}
