using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Attractions.Services;

public sealed class AttractionService(
    IRepository<Attraction> attractionRepository,
    IRepository<VisitorProfile> visitorProfileRepository,
    IRepository<Ticket> ticketRepository,
    IRepository<Event> eventRepository,
    IRepository<VisitRegistration> visitRegistrationRepository,
    IClockAppService clockAppService,
    IIncidenceService incidenceService) : IAttractionService
{
    private readonly IRepository<Attraction> _attractionRepository = attractionRepository;
    private readonly IRepository<Event> _eventRepository = eventRepository;
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    private readonly IClockAppService _clock = clockAppService;
    private readonly IIncidenceService _incidenceService = incidenceService;

    public Guid Create(AttractionArgs args)
    {
        Attraction attraction = MapToEntity(args);

        _attractionRepository.Add(attraction);

        return attraction.Id;
    }

    public List<Attraction> GetAll()
    {
        return _attractionRepository.GetAll(a => !a.IsDeleted);
    }

    public Attraction? Get(Guid id)
    {
        return _attractionRepository.Get(a => a.Id == id);
    }

    public void Update(AttractionArgs args, Guid id)
    {
        Attraction attraction = Get(id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        ApplyArgsToEntity(attraction, args);
        attraction.IsDeleted = false;

        _attractionRepository.Update(attraction);
    }

    public void Remove(Guid id)
    {
        Attraction attraction = Get(id)
                                ?? throw new InvalidOperationException($"Attraction with id {id} not found.");

        var now = _clock.Now();

        ValidateNoActiveIncidence(id, now);

        var events = _eventRepository.GetAll(e => e.Attractions.Any(a => a.Id == id));

        ValidateNoFutureEvents(events, now);

        attraction.IsDeleted = true;

        _attractionRepository.Update(attraction);
    }

    private static void ValidateNoFutureEvents(List<Event> events, DateTime now)
    {
        if(events.Any(e => e.Date > now))
        {
            throw new InvalidOperationException(
                "Attraction cannot be deleted because it is associated with a future event.");
        }
    }

    private void ValidateNoActiveIncidence(Guid id, DateTime now)
    {
        if(_incidenceService.HasActiveIncidenceForAttraction(id, now))
        {
            throw new InvalidOperationException(
                "Attraction cannot be deleted because it has active incidences.");
        }
    }

    public List<string> AttractionsReport(DateTime from, DateTime to)
    {
        ValidateDateRange(from, to);

        var attractions = GetAll();
        var visits = GetAllVisitRegistrations();

        var visitsInRange = FilterVisitsInRange(visits, from, to);

        var result = new List<string>();
        foreach(var attraction in attractions)
        {
            var count = CountVisitsForAttraction(visitsInRange, attraction.Id);
            result.Add($"{attraction.Name}\t{count}");
        }

        return result;
    }

    private static void ValidateDateRange(DateTime from, DateTime to)
    {
        if(from > to)
        {
            throw new ArgumentException("From date must be less than or equal to To date.");
        }
    }

    private static List<VisitRegistration> FilterVisitsInRange(List<VisitRegistration> visits, DateTime from, DateTime to)
    {
        var filtered = new List<VisitRegistration>();
        foreach(var v in visits)
        {
            if(v != null && v.Attractions != null && v.Attractions.Count > 0)
            {
                if(v.Date >= from && v.Date <= to)
                {
                    filtered.Add(v);
                }
            }
        }

        return filtered;
    }

    private static int CountVisitsForAttraction(List<VisitRegistration> visitsInRange, Guid attractionId)
    {
        var count = 0;
        foreach(var v in visitsInRange)
        {
            var includes = false;
            foreach(var a in v.Attractions)
            {
                if(a != null && a.Id == attractionId)
                {
                    includes = true;
                    break;
                }
            }

            if(includes)
            {
                count++;
            }
        }

        return count;
    }

    private List<VisitRegistration> GetAllVisitRegistrations()
    {
        return _visitRegistrationRepository.GetAll(
            null,
            v => v
                .Include(vr => vr.Attractions)
                .Include(vr => vr.ScoreEvents));
    }

    public static void ApplyArgsToEntity(Attraction entity, AttractionArgs args)
    {
        entity.Type = args.Type;
        entity.Name = args.Name;
        entity.MiniumAge = args.MiniumAge;
        entity.Capacity = args.Capacity;
        entity.Description = args.Description;
        entity.CurrentVisitors = args.CurrentVisitor;
        entity.Available = args.Available;
    }

    public void ValidateAttractionName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Attraction name cannot be empty.", nameof(name));
        }

        if(_attractionRepository.Exist(a => a.Name.ToLower() == name.ToLower()))
        {
            throw new Exception("Attraction name already exists.");
        }
    }

    public Attraction MapToEntity(AttractionArgs args)
    {
        ValidateAttractionName(args.Name);
        ValidationServices.ValidateAge(args.MiniumAge);

        var attraction = new Attraction
        {
            Name = args.Name,
            Type = args.Type,
            Description = args.Description,
            MiniumAge = args.MiniumAge,
            Capacity = args.Capacity,
            CurrentVisitors = args.CurrentVisitor,
            Available = args.Available
        };

        return attraction;
    }

    public bool ValidateEntryByNfc(Guid attractionId, Guid visitorId)
    {
        Attraction? attraction = _attractionRepository.Get(a => a.Id == attractionId);
        VisitorProfile? visitor = _visitorProfileRepository.Get(v => v.Id == visitorId);

        if(attraction is null || visitor is null)
        {
            return false;
        }

        if(!attraction.Available)
        {
            return false;
        }

        if(IsAtCapacity(attraction))
        {
            return false;
        }

        if(!IsOldEnough(visitor, attraction.MiniumAge))
        {
            return false;
        }

        if(!AttractionIsUnderIncidence(attractionId))
        {
            return false;
        }

        VisitRegistration? visitRegistration = _visitRegistrationRepository.Get(v => v.VisitorId == visitorId);

        visitRegistration = ValidateVisitRegistration(visitorId, visitRegistration, attraction);

        if(visitRegistration.IsActive)
        {
            return false;
        }

        attraction.CurrentVisitors++;
        _attractionRepository.Update(attraction);
        visitRegistration.IsActive = true;
        _visitRegistrationRepository.Update(visitRegistration);

        return true;
    }

    private bool AttractionIsUnderIncidence(Guid attractionId)
    {
        if(_incidenceService.HasActiveIncidenceForAttraction(attractionId, _clock.Now()))
        {
            return false;
        }

        return true;
    }

    private VisitRegistration ValidateVisitRegistration(Guid visitorId, VisitRegistration? visitRegistration,
        Attraction attraction)
    {
        if(visitRegistration != null)
        {
            return visitRegistration;
        }

        visitRegistration = new VisitRegistration
        {
            VisitorId = visitorId,
            Attractions = [attraction],
            Date = _clock.Now().Date,
            IsActive = false,
            Ticket = null!,
            TicketId = Guid.Empty
        };

        _visitRegistrationRepository.Add(visitRegistration);

        return visitRegistration;
    }

    private static bool IsAtCapacity(Attraction attraction)
    {
        return attraction.CurrentVisitors >= attraction.Capacity;
    }

    private bool IsOldEnough(VisitorProfile visitor, int minAge)
    {
        var today = DateOnly.FromDateTime(_clock.Now().Date);
        var age = today.Year - visitor.DateOfBirth.Year;

        if(visitor.DateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age >= minAge;
    }

    public bool ValidateEntryByQr(Guid attractionId, Guid qrId)
    {
        Attraction? attraction = _attractionRepository.Get(a => a.Id == attractionId);
        Ticket? ticket = _ticketRepository.Get(t => t.QrId == qrId);

        if(ticket is null || !IsTicketValidToday(ticket))
        {
            return false;
        }

        if(attraction is null || IsAtCapacity(attraction))
        {
            return false;
        }

        if(!AttractionIsUnderIncidence(attractionId))
        {
            return false;
        }

        Guid visitorId = ticket.Visitor.Id;

        VisitRegistration? visitRegistration = _visitRegistrationRepository.Get(v => v.VisitorId == visitorId);

        if(visitRegistration == null)
        {
            visitRegistration = CreateVisitRegistration(visitorId, ticket, attraction);

            _visitRegistrationRepository.Add(visitRegistration);
        }
        else
        {
            if(visitRegistration.IsActive)
            {
                return false;
            }

            visitRegistration.IsActive = true;
            visitRegistration.Attractions.Add(attraction);
            visitRegistration.Date = _clock.Now().Date;

            _visitRegistrationRepository.Update(visitRegistration);
        }

        return ticket.Type switch
        {
            EntranceType.Event => ValidateEventEntry(ticket, attraction),
            EntranceType.General => RegisterVisitor(attraction),
            _ => false
        };
    }

    private VisitRegistration CreateVisitRegistration(Guid visitorId, Ticket ticket, Attraction attraction)
    {
        var visitRegistration = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = ticket.Visitor,
            Date = _clock.Now().Date,
            IsActive = true,
            Attractions = [attraction],
            TicketId = ticket.Id,
            Ticket = ticket
        };
        return visitRegistration;
    }

    private bool IsTicketValidToday(Ticket ticket)
    {
        return ticket.Date.Date == _clock.Now().Date;
    }

    public bool ValidateEventEntry(Ticket ticket, Attraction attraction)
    {
        Event? ev = _eventRepository.Get(e => e.Id == ticket.EventId);
        if(ev is null || !IsAttractionInEvent(ev, attraction.Id))
        {
            return false;
        }

        var now = _clock.Now();
        DateTime startWindow = ev.Date;
        DateTime endWindow = ev.Date.AddHours(4);

        if(ticket.Date < startWindow.AddSeconds(-1) || now > endWindow)
        {
            return false;
        }

        List<Ticket> issuedTickets = _ticketRepository.GetAll(t => t.EventId == ticket.EventId);
        return issuedTickets.Count < ev.Capacity && RegisterVisitor(attraction);
    }

    private static bool IsAttractionInEvent(Event ev, Guid attractionId)
    {
        return ev.Attractions.Any(a => a.Id == attractionId);
    }

    private bool RegisterVisitor(Attraction attraction)
    {
        attraction.CurrentVisitors++;
        _attractionRepository.Update(attraction);
        return true;
    }

    public List<Attraction> GetDeleted()
    {
        return _attractionRepository.GetAll(a => a.IsDeleted);
    }
}
