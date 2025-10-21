using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Events.Services;

public class EventService(IRepository<Event> eventRepository, IRepository<Attraction> attractionRepository) : IEventService
{
    private readonly IRepository<Event> _eventRepository = eventRepository;
    private readonly IRepository<Attraction> _attractionRepository = attractionRepository;

    public Guid Create(EventsArgs args)
    {
        var entity = MapToEntity(args);
        _eventRepository.Add(entity);
        return entity.Id;
    }

    private Event MapToEntity(EventsArgs args)
    {
        List<Attraction>? attractions = null;

        if(args.AttractionIds.Count > 0)
        {
            attractions = MapAttractionsList(args.AttractionIds);
        }

        return new Event
        {
            Name = args.Name,
            Date = args.Date.ToDateTime(TimeOnly.MinValue),
            Capacity = args.Capacity,
            Cost = args.Cost,
            Attractions = attractions ?? []
        };
    }

    private List<Attraction> MapAttractionsList(List<Guid> argsAttractionIds)
    {
        var attractions = new List<Attraction>();

        foreach(var id in argsAttractionIds)
        {
            var attraction = _attractionRepository.Get(a => a.Id == id);

            if(attraction is null)
            {
                throw new InvalidOperationException($"Attraction with id {id} not found.");
            }

            attractions.Add(attraction);
        }

        return attractions;
    }

    public Event? Get(Guid eventId)
    {
        return _eventRepository.Get(e => e.Id == eventId);
    }

    public List<Event> GetAll()
    {
        List<Event> events = _eventRepository.GetAll();
        if(events == null)
        {
            throw new InvalidOperationException("Do not have any events");
        }

        return events;
    }

    public void Remove(Guid eventId)
    {
        var ev = _eventRepository.Get(
                     e => e.Id == eventId,
                     include: q => q.Include(e => e.Attractions))
                 ?? throw new InvalidOperationException($"Event with id {eventId} not found.");

        ev.Attractions.Clear();
        _eventRepository.Update(ev);

        _eventRepository.Remove(ev);
    }

    public void Update(EventsArgs args, Guid existingId)
    {
        var ev = _eventRepository.Get(e => e.Id == existingId)
                 ?? throw new InvalidOperationException($"Event with id {existingId} not found.");

        ApplyArgsToEntity(args, ev);

        _eventRepository.Update(ev);
    }

    private void ApplyArgsToEntity(EventsArgs args, Event ev)
    {
        ev.Name = args.Name;
        ev.Date = args.Date.ToDateTime(TimeOnly.MinValue);
        ev.Capacity = args.Capacity;
        ev.Cost = args.Cost;
        ev.Attractions = MapAttractionsList(args.AttractionIds);
    }

    public bool Exist(Guid eventId)
    {
        return _eventRepository.Exist(e => e.Id == eventId);
    }
}
