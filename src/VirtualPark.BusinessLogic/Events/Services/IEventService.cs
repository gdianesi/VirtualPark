using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;

namespace VirtualPark.BusinessLogic.Events.Services;

public interface IEventService
{
    public Guid Create(EventsArgs args);
    public Event? Get(Guid id);
    public List<Event> GetAll();
    public void Remove(Guid id);
    public void Update(EventsArgs args, Guid userId);
}
