using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.TypeIncidences.Service;

public sealed class TypeIncidenceService(IRepository<TypeIncidence> typeIncidenceRepository)
{
    private readonly IRepository<TypeIncidence> _typeIncidenceRepository = typeIncidenceRepository;
}
