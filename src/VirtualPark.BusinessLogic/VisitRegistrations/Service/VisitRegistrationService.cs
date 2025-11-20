using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public class VisitRegistrationService(IRepository<VisitRegistration> visitRegistrationRepository,
    IReadOnlyRepository<VisitorProfile> visitorProfileRepository, IReadOnlyRepository<Attraction> attractionRepository,
    IReadOnlyRepository<Ticket> ticketRepository, IClockAppService clockAppService,
    IRepository<VisitorProfile> visitorProfileWriteRepository, IStrategyService strategyService,
    IStrategyFactory strategyFactory, IReadOnlyRepository<Event> eventRepository) : IVisitRegistrationService
{
    private readonly IRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    private readonly IReadOnlyRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IReadOnlyRepository<Attraction> _attractionRepository = attractionRepository;
    private readonly IReadOnlyRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly IClockAppService _clockAppService = clockAppService;
    private readonly IRepository<VisitorProfile> _visitorProfileWriteRepository = visitorProfileWriteRepository;
    private readonly IStrategyService _strategyService = strategyService;
    private readonly IStrategyFactory _strategyFactory = strategyFactory;
    private readonly IReadOnlyRepository<Event> _eventRepository = eventRepository;

    public VisitRegistration Create(VisitRegistrationArgs args)
    {
        var entity = MapToEntity(args);

        _visitRegistrationRepository.Add(entity);

        return entity;
    }

    public VisitRegistration? Get(Guid id)
    {
        var visitRegistration = _visitRegistrationRepository.Get(v => v.Id == id) ?? throw new InvalidOperationException("Visitor don't exist");
        visitRegistration.Visitor = SearchVisitorProfile(visitRegistration.VisitorId);
        visitRegistration.Attractions = RefreshAttractions(visitRegistration.Attractions);
        visitRegistration.Ticket = SearchTicket(visitRegistration.TicketId);
        return visitRegistration;
    }

    public void Remove(Guid id)
    {
        var visitRegistration = _visitRegistrationRepository.Get(v => v.Id == id) ?? throw new InvalidOperationException("Visitor don't exist");
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
        var visitRegistrations = _visitRegistrationRepository.GetAll() ?? throw new InvalidOperationException("Dont have any visit registrations");
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

    public void UpToAttraction(Guid visitorId, Guid attractionId)
    {
        var today = DateOnly.FromDateTime(_clockAppService.Now());

        var visitRegistration = GetTodayVisitForVisitor(visitorId, today);

        if(visitRegistration.CurrentAttractionId is not null)
        {
            throw new InvalidOperationException("Visitor is already on an attraction.");
        }

        var attraction = _attractionRepository.Get(a => a.Id == attractionId) ?? throw new InvalidOperationException("Attraction don't exist");
        visitRegistration.CurrentAttraction = attraction;
        visitRegistration.CurrentAttractionId = attraction.Id;

        _visitRegistrationRepository.Update(visitRegistration);
    }

    public void DownToAttraction(Guid visitorId)
    {
        var today = DateOnly.FromDateTime(_clockAppService.Now());

        var visitRegistration = GetTodayVisitForVisitor(visitorId, today);

        if(visitRegistration.CurrentAttractionId is null)
        {
            return;
        }

        visitRegistration.CurrentAttraction = null;
        visitRegistration.CurrentAttractionId = null;
        visitRegistration.IsActive = false;

        _visitRegistrationRepository.Update(visitRegistration);
    }

    public List<Attraction> GetAttractionsForTicket(Guid visitorId)
    {
        var today = DateOnly.FromDateTime(_clockAppService.Now());

        var visit = GetTodayVisitForVisitor(visitorId, today);
        visit.Ticket ??= SearchTicket(visit.TicketId);
        visit.Visitor ??= SearchVisitorProfile(visit.VisitorId);
        visit.Attractions = RefreshAttractions(visit.Attractions);

        var ticket = EnsureTicketLoaded(visit);

        return GetAttractionsForTicketInternal(ticket);
    }

    private VisitRegistration GetTodayVisitForVisitor(Guid visitorId, DateOnly today)
    {
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visit = _visitRegistrationRepository.Get(v =>
            v.VisitorId == visitorId &&
            v.Date >= start &&
            v.Date <= end) ?? throw new InvalidOperationException("VisitRegistration for today don't exist");
        visit.Visitor = SearchVisitorProfile(visit.VisitorId);
        visit.Attractions = RefreshAttractions(visit.Attractions);
        visit.Ticket = SearchTicket(visit.TicketId);

        return visit;
    }

    private Ticket EnsureTicketLoaded(VisitRegistration visit)
    {
        if(visit.Ticket is null)
        {
            throw new InvalidOperationException("Ticket don't exist");
        }

        return visit.Ticket;
    }

    private List<Attraction> GetAttractionsForTicketInternal(Ticket ticket)
    {
        return ticket.Type switch
        {
            EntranceType.General => GetAllAttractionsFromRepository(),
            EntranceType.Event => GetEventAttractions(ticket),
            _ => throw new InvalidOperationException($"Unsupported ticket type: {ticket.Type}")
        };
    }

    private List<Attraction> GetAllAttractionsFromRepository()
    {
        var attractions = _attractionRepository.GetAll() ?? throw new InvalidOperationException("Dont have any attractions");
        return attractions;
    }

    private List<Attraction> GetEventAttractions(Ticket ticket)
    {
        var eventId = ticket.EventId ?? ticket.Event?.Id
            ?? throw new InvalidOperationException("Ticket event don't have attractions");

        var ev = _eventRepository.Get(
                     e => e.Id == eventId,
                     include: q => q.Include(e => e.Attractions))
                 ?? throw new InvalidOperationException("Event don't exist");

        return RefreshAttractions(ev.Attractions);
    }

    private Ticket SearchTicket(Guid id)
    {
        var ticket = _ticketRepository.Get(t => t.Id == id) ?? throw new InvalidOperationException("Ticket don't exist");
        return ticket;
    }

    private VisitorProfile SearchVisitorProfile(Guid id)
    {
        var visitor = _visitorProfileRepository.Get(v => v.Id == id) ?? throw new InvalidOperationException("Visitor don't exist");
        return visitor;
    }

    private List<Attraction> SearchAttractions(List<Guid> attractionsIds)
    {
        List<Attraction> attractions = [];
        foreach(var attractionId in attractionsIds)
        {
            var attraction = _attractionRepository.Get(x => x.Id == attractionId) ?? throw new InvalidOperationException("Attraction don't exist");
            attractions.Add(attraction);
        }

        return attractions;
    }

    private List<Attraction> RefreshAttractions(List<Attraction> attractionsOnlyId)
    {
        List<Attraction> attractions = [];
        foreach(var a in attractionsOnlyId)
        {
            var attraction = _attractionRepository.Get(x => x.Id == a.Id) ?? throw new InvalidOperationException("Attraction don't exist");
            attractions.Add(new Attraction
            {
                Id = attraction.Id,
                Name = attraction.Name,
                Description = attraction.Description,
                Available = attraction.Available,
                Capacity = attraction.Capacity,
                CurrentVisitors = attraction.CurrentVisitors,
                MiniumAge = attraction.MiniumAge,
                Type = attraction.Type
            });
        }

        return attractions;
    }

    public void RecordVisitScore(RecordVisitScoreArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        if(string.IsNullOrWhiteSpace(args.Origin))
        {
            throw new InvalidOperationException("Origin es requerido.");
        }

        var now = _clockAppService.Now();
        var today = DateOnly.FromDateTime(now);

        var visit = GetVisitById(args.VisitRegistrationId);

        var strategyKey = GetStrategyKeyForVisit(visit, today);

        var scoreEvent = new VisitScore
        {
            Origin = args.Origin.Trim(),
            OccurredAt = now,
            Points = 0,
            DayStrategyName = strategyKey,
            VisitRegistrationId = visit.Id,
            VisitRegistration = visit
        };
        visit.ScoreEvents.Add(scoreEvent);

        var previousTotal = visit.DailyScore;
        var newTotal = ComputeNewTotal(visit, strategyKey, args);

        var delta = newTotal - previousTotal;
        ApplyDelta(visit, scoreEvent, delta, newTotal);

        _visitRegistrationRepository.Update(visit);
    }

    private VisitRegistration GetVisitById(Guid visitRegistrationId)
    {
        var visit = _visitRegistrationRepository.Get(v => v.Id == visitRegistrationId)
                   ?? throw new InvalidOperationException($"VisitRegistration {visitRegistrationId} not found.");

        visit.Visitor ??= SearchVisitorProfile(visit.VisitorId);
        visit.Attractions = RefreshAttractions(visit.Attractions);
        visit.Ticket ??= SearchTicket(visit.TicketId);

        return visit;
    }

    private string GetStrategyKeyForVisit(VisitRegistration visit, DateOnly today)
    {
        var keyFromHistory = visit.ScoreEvents.FirstOrDefault()?.DayStrategyName;
        if(!string.IsNullOrWhiteSpace(keyFromHistory))
        {
            return keyFromHistory!;
        }

        var active = _strategyService.Get(today)
                     ?? throw new InvalidOperationException($"No active strategy for {today}.");

        return active.StrategyKey;
    }

    private int ComputeNewTotal(VisitRegistration visit, string strategyKey, RecordVisitScoreArgs args)
    {
        var isRedemption = string.Equals(args.Origin, "Canje", StringComparison.OrdinalIgnoreCase);

        if(isRedemption)
        {
            if(args.Points is null)
            {
                throw new InvalidOperationException("Points es requerido para origen 'Canje'.");
            }

            return checked(visit.DailyScore + args.Points.Value);
        }

        if(args.Points is not null)
        {
            throw new InvalidOperationException("Points solo se permite para 'Canje'; para otros orígenes deje null.");
        }

        var strategy = _strategyFactory.Create(strategyKey);

        return strategy.CalculatePoints(visit.VisitorId);
    }

    private void ApplyDelta(VisitRegistration visit, VisitScore scoreEvent, int delta, int newTotal)
    {
        scoreEvent.Points = delta;

        if(delta == 0)
        {
            return;
        }

        visit.Visitor ??= _visitorProfileRepository.Get(v => v.Id == visit.VisitorId)
                          ?? throw new InvalidOperationException("Visitor not found.");

        visit.DailyScore = newTotal;
        visit.Visitor.Score += delta;
        visit.Visitor.PointsAvailable += delta;

        _visitorProfileWriteRepository.Update(visit.Visitor);
    }

    public List<VisitorInAttraction> GetVisitorsInAttraction(Guid attractionId)
    {
        var visits = _visitRegistrationRepository.GetAll() ?? throw new InvalidOperationException("Dont have any visit registrations");
        var today = DateOnly.FromDateTime(_clockAppService.Now());

        var todayVisitsInAttraction = visits
            .Where(v =>
                v.IsActive &&
                v.CurrentAttractionId == attractionId &&
                DateOnly.FromDateTime(v.Date) == today)
            .ToList();

        var result = new List<VisitorInAttraction>();

        foreach(var visit in todayVisitsInAttraction)
        {
            var visitor = visit.Visitor ?? SearchVisitorProfile(visit.VisitorId);
            var ticket = visit.Ticket ?? SearchTicket(visit.TicketId);

            result.Add(new VisitorInAttraction
            {
                VisitRegistrationId = visit.Id,
                Visitor = visitor,
                TicketType = ticket.Type
            });
        }

        return result;
    }
}
