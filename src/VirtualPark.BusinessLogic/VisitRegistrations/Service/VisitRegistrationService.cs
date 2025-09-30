using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public class VisitRegistrationService(IRepository<VisitRegistration> visitRegistrationRepository,
    IReadOnlyRepository<VisitorProfile> visitorProfileRepository, IReadOnlyRepository<Attraction> attractionRepository,
    IReadOnlyRepository<Ticket> ticketRepository)
{
    private readonly IRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    private readonly IReadOnlyRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IReadOnlyRepository<Attraction> _attractionRepository = attractionRepository;
    private readonly IReadOnlyRepository<Ticket> _ticketRepository = ticketRepository;
    public VisitRegistration Create(VisitRegistrationArgs args)
    {
        var entity = MapToEntity(args);

        _visitRegistrationRepository.Add(entity);

        return entity;
    }

    private VisitRegistration MapToEntity(VisitRegistrationArgs args)
    {
        var visitor = _visitorProfileRepository.Get(v => v.Id == args.VisitorProfileId);
        if (visitor is null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        List<Attraction> attractions = new List<Attraction>();
        foreach(var attractionId in args.AttractionsId)
        {
            var attraction = _attractionRepository.Get(x => x.Id == attractionId);
            if(attraction is null)
            {
                throw new InvalidOperationException("Attraction don't exist");
            }

            attractions.Add(attraction);
        }

        var ticket = _ticketRepository.Get(t => t.Id == args.TicketId);
        if (ticket is null)
        {
            throw new InvalidOperationException("Ticket don't exist");
        }

        return new VisitRegistration
        {
            VisitorId = visitor.Id,
            Visitor = visitor,
            Attractions = attractions,
            Ticket = ticket,
            TicketId = ticket.Id
        };
    }
}
