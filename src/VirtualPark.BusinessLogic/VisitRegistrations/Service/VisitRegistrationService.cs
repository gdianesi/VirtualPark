using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
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

    public VisitRegistration? Get(Guid id)
    {
        var visitRegistration = _visitRegistrationRepository.Get(v => v.Id == id);

        if(visitRegistration == null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        visitRegistration.Visitor = SearchVisitorProfile(visitRegistration.VisitorId);
        visitRegistration.Attractions = RefreshAttractions(visitRegistration.Attractions);
        visitRegistration.Ticket = SearchTicket(visitRegistration.TicketId);
        return visitRegistration;
    }

    private VisitRegistration MapToEntity(VisitRegistrationArgs args)
    {
        var visitor = SearchVisitorProfile(args.VisitorProfileId);

        List<Attraction> attractions = SearchAttractions(args.AttractionsId);

        var ticket = SearchTicket(args.TicketId);

        return new VisitRegistration
        {
            VisitorId = visitor.Id,
            Visitor = visitor,
            Attractions = attractions,
            Ticket = ticket,
            TicketId = ticket.Id
        };
    }

    private Ticket SearchTicket(Guid id)
    {
        var ticket = _ticketRepository.Get(t => t.Id == id);
        if (ticket is null)
        {
            throw new InvalidOperationException("Ticket don't exist");
        }

        return ticket;
    }

    private VisitorProfile SearchVisitorProfile(Guid id)
    {
        var visitor = _visitorProfileRepository.Get(v => v.Id == id);
        if (visitor is null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        return visitor;
    }

    private List<Attraction> SearchAttractions(List<Guid> attractionsIds)
    {
        List<Attraction> attractions = new List<Attraction>();
        foreach(var attractionId in attractionsIds)
        {
            var attraction = _attractionRepository.Get(x => x.Id == attractionId);
            if(attraction is null)
            {
                throw new InvalidOperationException("Attraction don't exist");
            }

            attractions.Add(attraction);
        }

        return attractions;
    }

    private List<Attraction> RefreshAttractions(List<Attraction> attractionsOnlyId)
    {
        List<Attraction> attractions = new List<Attraction>();
        foreach(var a in attractionsOnlyId)
        {
            var attraction = _attractionRepository.Get(x => x.Id == a.Id);
            if(attraction is null)
            {
                throw new InvalidOperationException("Attraction don't exist");
            }

            attractions.Add(attraction);
        }

        return attractions;
    }
}
