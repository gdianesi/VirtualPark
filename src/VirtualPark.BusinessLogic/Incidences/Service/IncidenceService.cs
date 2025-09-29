using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public sealed class IncidenceService(IRepository<Incidence> incidenceRepository, IReadOnlyRepository<TypeIncidence> incidenceReadOnlyTypeRepository)
{
    private readonly IRepository<Incidence> _incidenceRepository = incidenceRepository;
    private readonly IReadOnlyRepository<TypeIncidence> _typeIncidenceRepository = incidenceReadOnlyTypeRepository;
    
}
