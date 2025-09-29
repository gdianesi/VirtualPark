using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Events.Services;

public class EventService(IRepository<Event> eventrepository, IRepository<Attraction> attractionrepository)
{
    private readonly IRepository<Attraction> _attractionRepository = attractionrepository;
    private readonly IRepository<Event> _eventRepository = eventrepository;

    public Guid Create(EventsArgs args)
    {
        List<Attraction> attractions = ValidateAndLoadAttractions(args.AttractionIds);
        Event? entity = MapToEntity(args, attractions);
        _eventRepository.Add(entity);
        return entity.Id;
    }

    private static Event MapToEntity(EventsArgs args, List<Attraction> attractions)
    {
        return new Event
        {
            Name = args.Name,
            Date = args.Date.ToDateTime(TimeOnly.MinValue),
            Capacity = args.Capacity,
            Cost = args.Cost,
            Attractions = attractions
        };
    }

    public void Remove(Guid id)
    {
        Event existingEvent = _eventRepository.Get(a => a.Id == id) ??
                              throw new InvalidOperationException($"Event with id {id} not found.");
        _eventRepository.Remove(existingEvent);
    }

    public List<Event> GetAll(Expression<Func<Event, bool>>? predicate = null)
    {
        if(predicate == null)
        {
            return _eventRepository.GetAll().ToList();
        }

        return _eventRepository.GetAll(predicate).ToList();
    }

    public void Update(EventsArgs args, Guid id)
    {
        Event? ev = _eventRepository.Get(e => e.Id == id);

        if(ev != null)
        {
            List<Attraction>? attractions = ValidateAndLoadAttractions(args.AttractionIds);
            ApplyArgsToEntity(ev, args, attractions);
            _eventRepository.Update(ev);
        }
    }

    private static void ApplyArgsToEntity(Event entity, EventsArgs args, List<Attraction> attractions)
    {
        entity.Name = args.Name;
        entity.Date = args.Date.ToDateTime(TimeOnly.MinValue);
        entity.Capacity = args.Capacity;
        entity.Cost = args.Cost;
        entity.Attractions = attractions;
    }

    private List<Attraction> ValidateAndLoadAttractions(List<Guid> attractionIds)
    {
        var attractions = new List<Attraction>();

        foreach(Guid id in attractionIds)
        {
            Attraction? attraction = _attractionRepository.Get(a => a.Id == id);
            if(attraction == null)
            {
                throw new InvalidOperationException($"Attraction with id {id} not found.");
            }

            attractions.Add(attraction);
        }

        return attractions;
    }
}
