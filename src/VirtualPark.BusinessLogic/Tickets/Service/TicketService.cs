using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Tickets.Service;

public class TicketService(
    IRepository<Ticket> ticketRepository,
    IRepository<VisitorProfile> visitorProfileRepository,
    IRepository<Event> eventRepository,
    IClockAppService clockAppService) : ITicketService
{
    private readonly IRepository<Event> _eventRepository = eventRepository;
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IClockAppService _clockAppService = clockAppService;

    public Guid Create(TicketArgs args)
    {
        Ticket ticket = MapToEntity(args);
        _ticketRepository.Add(ticket);
        return ticket.Id;
    }

    public void Remove(Guid ticketId)
    {
        Ticket ticket = Get(ticketId)
                        ?? throw new InvalidOperationException($"Ticket with id {ticketId} not found.");

        _ticketRepository.Remove(ticket);
    }

    public Ticket? Get(Guid ticketId)
    {
        return _ticketRepository.Get(t => t.Id == ticketId);
    }

    public List<Ticket> GetAll()
    {
        return _ticketRepository.GetAll();
    }

    private Ticket MapToEntity(TicketArgs args)
    {
        var visitor = GetVisitorEntity(args)
                      ?? throw new InvalidOperationException($"Visitor with id {args.VisitorId} not found.");

        var ticket = new Ticket
        {
            Date = args.Date,
            Type = args.Type,
            Visitor = visitor,
            VisitorProfileId = args.VisitorId,
            QrId = Guid.NewGuid(),
            EventId = null
        };

        if(args.EventId.HasValue)
        {
            var eventEntity = _eventRepository.Get(e => e.Id == args.EventId.Value)
                              ?? throw new InvalidOperationException($"Event with id {args.EventId} not found.");

            ticket.Event = eventEntity;
            ticket.EventId = args.EventId.Value;
        }

        return ticket;
    }

    private VisitorProfile? GetVisitorEntity(TicketArgs args)
    {
        return _visitorProfileRepository.Get(v => v.Id == args.VisitorId);
    }

    public bool HasTicketForVisitor(Guid visitorId)
    {
        return _ticketRepository.Exist(t => t.Visitor.Id == visitorId);
    }

    public bool IsTicketValidForEntry(Guid qrId)
    {
        Ticket ticket = GetTicket(qrId);

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

    private Ticket GetTicket(Guid qrId)
    {
        return _ticketRepository.Get(t => t.QrId == qrId)
               ?? throw new InvalidOperationException($"No ticket found with QR: {qrId}");
    }

    private bool IsDateValid(DateTime ticketDate)
    {
        var now = _clockAppService.Now();
        return ticketDate.Date == now.Date;
    }

    private bool IsEventValid(Ticket ticket)
    {
        Event ev = _eventRepository.Get(e => e.Id == ticket.EventId)
                   ?? throw new InvalidOperationException($"Event with id {ticket.EventId} not found.");

        var issuedCount = _ticketRepository.GetAll(t => t.EventId == ticket.EventId).Count;
        return issuedCount < ev.Capacity;
    }
}
