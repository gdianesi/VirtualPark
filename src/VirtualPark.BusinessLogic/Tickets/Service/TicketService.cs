using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Tickets.Service;

public class TicketService(IRepository<Ticket> ticketRepository, IRepository<VisitorProfile> visitorProfileRepository, IRepository<Event> eventRepository)
{
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IRepository<Event> _eventRepository = eventRepository;

    public Ticket Create(TicketArgs args)
    {
        Ticket ticket = MapToEntity(args);
        _ticketRepository.Add(ticket);
        return ticket;
    }

    private Ticket MapToEntity(TicketArgs args)
    {
        var visitor = GetVisitorEntity(args);
        var ticket = new Ticket { Date = args.Date, Type = args.Type, EventId = args.EventId, Visitor = visitor! };
        return ticket;
    }

    private VisitorProfile? GetVisitorEntity(TicketArgs args)
    {
        return _visitorProfileRepository.Get(v => v.Id == args.VisitorId);
    }

    public void Remove(Guid ticketId)
    {
        var ticket = Get(a => a.Id == ticketId)
                     ?? throw new InvalidOperationException($"Ticket with id {ticketId} not found.");

        _ticketRepository.Remove(ticket);
    }

    public Ticket? Get(Expression<Func<Ticket, bool>> predicate)
    {
        return _ticketRepository.Get(predicate);
    }

    public List<Ticket> GetAll(Expression<Func<Ticket, bool>>? predicate = null)
    {
        return _ticketRepository.GetAll(predicate);
    }

    public bool HasTicketForVisitor(Guid visitorId)
    {
        return _ticketRepository.Exist(t => t.Visitor.Id == visitorId);
    }

    public bool IsTicketValidForEntry(Guid qrId)
    {
        var ticket = GetTicket(qrId);

        if(!IsDateValid(ticket.Date))
        {
            return false;
        }

        return ticket.Type switch
        {
            EntranceType.General => true,
            EntranceType.Event => IsEventValid(ticket),
            _ => false
        };
    }

    private Ticket GetTicket(Guid qrId) =>
        _ticketRepository.Get(t => t.QrId == qrId)
        ?? throw new InvalidOperationException($"No ticket found with QR: {qrId}");

    private static bool IsDateValid(DateTime ticketDate) =>
        ticketDate == DateTime.Today;

    private bool IsEventValid(Ticket ticket)
    {
        var ev = _eventRepository.Get(e => e.Id == ticket.EventId)
                 ?? throw new InvalidOperationException($"Event with id {ticket.EventId} not found.");

        var issuedCount = _ticketRepository.GetAll(t => t.EventId == ticket.EventId).Count;
        return issuedCount < ev.Capacity;
    }
}
