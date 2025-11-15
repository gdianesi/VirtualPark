using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;

namespace VirtualPark.BusinessLogic.Tickets.Service;

public interface ITicketService
{
    public Guid Create(TicketArgs args);
    public Ticket? Get(Guid id);
    public List<Ticket> GetAll();
    public void Remove(Guid id);
    List<Ticket> GetTicketsByVisitor(Guid visitorId);
}
