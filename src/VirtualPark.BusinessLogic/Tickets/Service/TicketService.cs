using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Tickets.Service;

public class TicketService(IRepository<Ticket> ticketRepository, VisitorProfileService visitorProfileService)
{
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly VisitorProfileService _visitorProfileService = visitorProfileService;

    public Ticket Create(TicketArgs args)
    {
        Ticket ticket = MapToEntity(args);
        _ticketRepository.Add(ticket);
        return ticket;
    }

    private Ticket MapToEntity(TicketArgs args)
    {
        VisitorProfile visitor = GetVisitorEntity(args);
        Ticket ticket = new Ticket { Date = args.Date, Type = args.Type, EventId = args.EventId, Visitor = visitor };
        return ticket;
    }

    private VisitorProfile? GetVisitorEntity(TicketArgs args)
    {
        return _visitorProfileService.Get(args.VisitorId);
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

    public bool IsTicketValidForEntry(Guid ticketId, Guid qrId)
    {
        var ticket = _ticketRepository.Get(t => t.QrId == qrId)
                     ?? throw new InvalidOperationException($"No ticket found with QR: {qrId}");

        if (ticket.Date != DateOnly.FromDateTime(DateTime.Today))
        {
            return false;
        }

        return ticket.Type is EntranceType.General or EntranceType.Event;
    }
}
