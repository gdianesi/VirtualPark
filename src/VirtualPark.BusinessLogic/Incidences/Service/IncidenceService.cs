using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public sealed class IncidenceService(IRepository<Incidence> incidenceRepository)
{
    private readonly IRepository<Incidence> _incidenceRepository = incidenceRepository;
}
