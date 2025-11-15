using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public interface IIncidenceService
{
    public Guid Create(IncidenceArgs args);
    public Incidence? Get(Guid id);
    public List<Incidence> GetAll();
    public void Remove(Guid id);
    public void Update(IncidenceArgs args, Guid incidenceId);
    bool HasActiveIncidenceForAttraction(Guid attractionId, DateTime dateTime);
}
