using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.ViaAccess.Entity;

public sealed class Qr(Ticket ticket) : IViaAccess
{
    private readonly Ticket _ticket = ticket;

    public Guid QrId { get; } = ticket.QrId;

    public VisitorProfile IdentifyVisitor() => _ticket.Visitor;
}
