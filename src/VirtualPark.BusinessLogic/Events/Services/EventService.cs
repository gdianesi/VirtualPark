using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Events.Services;

public class EventService(IRepository<Event> repository)
{
    private readonly IRepository<Event> _repository = repository;

    public Guid Create(EventsArgs args)
    {
        var entity = MapToEntity(args);
        _repository.Add(entity);
        return entity.Id;
    }

    private static Event MapToEntity(EventsArgs args) => new Event
    {
        Name = args.Name, Date = args.Date.ToDateTime(TimeOnly.MinValue), Capacity = args.Capacity, Cost = args.Cost,
    };

    public void Remove(Guid id)
    {
        Event existingEvent = _repository.Get(a => a.Id == id) ??
                              throw new InvalidOperationException($"Event with id {id} not found.");
        _repository.Remove(existingEvent);
    }
}
