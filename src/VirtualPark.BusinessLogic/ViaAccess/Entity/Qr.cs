using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.ViaAccess.Entity;

public sealed class Qr(Ticket ticket) : IViaAccess
{
    private readonly Ticket _ticket = ticket;

    public Guid QrId { get; } = ticket.QrId;

    public Visitor IdentifyVisitor() => _ticket.Visitor;
}
