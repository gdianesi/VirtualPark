using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.ViaAccess.Entity;

public class Qr : IViaAccess
{
    private readonly Ticket _ticket;

    public Qr(Ticket ticket)
    {
        _ticket = ticket;
    }

    public Visitor IdentifyVisitor()
    {
        return _ticket.Visitor;
    }
}
