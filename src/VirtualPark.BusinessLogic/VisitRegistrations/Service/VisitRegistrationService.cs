using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public class VisitRegistrationService(IRepository<VisitRegistration> visitRegistrationRepository,
    IReadOnlyRepository<VisitorProfile> visitorProfileRepository, IReadOnlyRepository<Attraction> attractionRepository,
    IReadOnlyRepository<Ticket> ticketRepository, IClockAppService clockAppService,
    IRepository<VisitorProfile> visitorProfileWriteRepository, IStrategyService strategyService,
    IStrategyFactory strategyFactory) : IVisitRegistrationService
{
    private readonly IRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    private readonly IReadOnlyRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IReadOnlyRepository<Attraction> _attractionRepository = attractionRepository;
    private readonly IReadOnlyRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly IClockAppService _clockAppService = clockAppService;
    private readonly IRepository<VisitorProfile> _visitorProfileWriteRepository = visitorProfileWriteRepository;
    private readonly IStrategyService _strategyService = strategyService;
    private readonly IStrategyFactory _strategyFactory = strategyFactory;

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

    public void Remove(Guid id)
    {
        var visitRegistration = _visitRegistrationRepository.Get(v => v.Id == id);

        if(visitRegistration == null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        _visitRegistrationRepository.Remove(visitRegistration);
    }

    public void Update(VisitRegistrationArgs args, Guid visitId)
    {
        var visitRegistration = Get(visitId);

        ApplyChange(visitRegistration!, args);

        _visitRegistrationRepository.Update(visitRegistration!);
    }

    private void ApplyChange(VisitRegistration visitorRegistration, VisitRegistrationArgs args)
    {
        visitorRegistration.Ticket = SearchTicket(args.TicketId);
        visitorRegistration.TicketId = args.TicketId;
        visitorRegistration.Visitor = SearchVisitorProfile(visitorRegistration.VisitorId);
        visitorRegistration.VisitorId = args.VisitorProfileId;
        visitorRegistration.Attractions = RefreshAttractions(visitorRegistration.Attractions);
    }

    public List<VisitRegistration> GetAll()
    {
        var visitRegistrations = _visitRegistrationRepository.GetAll();

        if(visitRegistrations == null)
        {
            throw new InvalidOperationException("Dont have any visit registrations");
        }

        UploadData(visitRegistrations);
        return visitRegistrations;
    }

    private List<VisitRegistration> UploadData(List<VisitRegistration> visitRegistrations)
    {
        foreach(var vr in visitRegistrations)
        {
            vr.Ticket = SearchTicket(vr.TicketId);
            vr.Visitor = SearchVisitorProfile(vr.VisitorId);
            vr.Attractions = RefreshAttractions(vr.Attractions);
        }

        return visitRegistrations;
    }

    private VisitRegistration MapToEntity(VisitRegistrationArgs args)
    {
        var visitor = SearchVisitorProfile(args.VisitorProfileId);

        List<Attraction> attractions = SearchAttractions(args.AttractionsId);

        var ticket = SearchTicket(args.TicketId);

        return new VisitRegistration
        {
            VisitorId = visitor.Id,
            Date = _clockAppService.Now(),
            Visitor = visitor,
            Attractions = attractions,
            Ticket = ticket,
            TicketId = ticket.Id
        };
    }

    private Ticket SearchTicket(Guid id)
    {
        var ticket = _ticketRepository.Get(t => t.Id == id);
        if(ticket is null)
        {
            throw new InvalidOperationException("Ticket don't exist");
        }

        return ticket;
    }

    private VisitorProfile SearchVisitorProfile(Guid id)
    {
        var visitor = _visitorProfileRepository.Get(v => v.Id == id);
        if(visitor is null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        return visitor;
    }

    private List<Attraction> SearchAttractions(List<Guid> attractionsIds)
    {
        List<Attraction> attractions = [];
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
        List<Attraction> attractions = [];
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

    private void CloseVisit(Guid visitId)
    {
        var visit = Get(visitId);

        if(visit == null)
        {
            throw new InvalidOperationException($"Visit with id {visitId} not found");
        }

        if(visit.DailyScore > 0)
        {
            throw new InvalidOperationException($"Points for this visit have already been calculated");
        }

        var dateOnly = DateOnly.FromDateTime(visit.Date);
        var activeStrategyArgs = _strategyService.Get(dateOnly);

        if(activeStrategyArgs == null)
        {
            throw new InvalidOperationException($"No active strategy found for date {dateOnly}");
        }

        var strategy = _strategyFactory.Create(activeStrategyArgs.StrategyKey);
        var points = strategy.CalculatePoints(visit);

        visit.DailyScore = points;

        visit.Visitor.Score += points;

        _visitRegistrationRepository.Update(visit);
        _visitorProfileWriteRepository.Update(visit.Visitor);
    }

    public void CloseVisitByVisitor(Guid visitorProfileId)
    {
        var today = DateOnly.FromDateTime(_clockAppService.Now());

        var activeVisit = _visitRegistrationRepository.Get(v =>
            v.VisitorId == visitorProfileId &&
            DateOnly.FromDateTime(v.Date) == today &&
            v.DailyScore == 0);

        if(activeVisit == null)
        {
            throw new InvalidOperationException($"No active visit found for visitor {visitorProfileId} today");
        }

        CloseVisit(activeVisit.Id);
    }
}
