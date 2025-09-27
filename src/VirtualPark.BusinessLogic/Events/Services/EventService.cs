using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Events.Services;

public class EventService(IRepository<Event> eventrepository)
{
    private readonly IRepository<Event> _eventRepository = eventrepository;

    public Guid Create(EventsArgs args)
    {
        var entity = MapToEntity(args);
        _eventRepository.Add(entity);
        return entity.Id;
    }

    private static Event MapToEntity(EventsArgs args) => new Event
    {
        Name = args.Name, Date = args.Date.ToDateTime(TimeOnly.MinValue), Capacity = args.Capacity, Cost = args.Cost,
    };

    public void Remove(Guid id)
    {
        Event existingEvent = _eventRepository.Get(a => a.Id == id) ??
                              throw new InvalidOperationException($"Event with id {id} not found.");
        _eventRepository.Remove(existingEvent);
    }

    public List<Event> GetAll(Expression<Func<Event, bool>>? predicate = null)
    {
        return _eventRepository.GetAll(predicate).ToList();
    }
}
