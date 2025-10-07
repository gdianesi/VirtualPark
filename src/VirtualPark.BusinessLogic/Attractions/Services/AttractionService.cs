using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Events.Entity;
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
    IRepository<VisitRegistration> visitRegistrationRepository) : IAttractionService
{
    private readonly IRepository<Attraction> _attractionRepository = attractionRepository;
    private readonly IRepository<Event> _eventRepository = eventRepository;
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;

    public Guid Create(AttractionArgs args)
    {
        Attraction attraction = MapToEntity(args);

        _attractionRepository.Add(attraction);

        return attraction.Id;
    }

    public List<Attraction> GetAll()
    {
        return _attractionRepository.GetAll();
    }

    public Attraction? Get(Guid id)
    {
        return _attractionRepository.Get(a => a.Id == id);
    }

    public void Update(AttractionArgs args, Guid id)
    {
        Attraction attraction = Get(id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        ApplyArgsToEntity(attraction, args);

        _attractionRepository.Update(attraction);
    }

    public void Remove(Guid id)
    {
        Attraction attraction = Get(id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        _attractionRepository.Remove(attraction);
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

        if(_attractionRepository.Exist(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
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
            Date = DateTime.Today,
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

    private static bool IsOldEnough(VisitorProfile visitor, int minAge)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
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
            visitRegistration.Date = DateTime.Today;

            _visitRegistrationRepository.Update(visitRegistration);
        }

        return ticket.Type switch
        {
            EntranceType.Event => ValidateEventEntry(ticket, attraction),
            EntranceType.General => RegisterVisitor(attraction),
            _ => false
        };
    }

    private static VisitRegistration CreateVisitRegistration(Guid visitorId, Ticket ticket, Attraction attraction)
    {
        var visitRegistration = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = ticket.Visitor,
            Date = DateTime.Today,
            IsActive = true,
            Attractions = [attraction],
            TicketId = ticket.Id,
            Ticket = ticket
        };
        return visitRegistration;
    }

    private static bool IsTicketValidToday(Ticket ticket)
    {
        return ticket.Date.Date == DateTime.Today;
    }

    public bool ValidateEventEntry(Ticket ticket, Attraction attraction)
    {
        Event? ev = _eventRepository.Get(e => e.Id == ticket.EventId);
        if(ev is null || !IsAttractionInEvent(ev, attraction.Id))
        {
            return false;
        }

        DateTime startWindow = ev.Date;
        DateTime endWindow = ev.Date.AddHours(4);

        if(ticket.Date < startWindow.AddSeconds(-1) || DateTime.Now > endWindow)
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
}
