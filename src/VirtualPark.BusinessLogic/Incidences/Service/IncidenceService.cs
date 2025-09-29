using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public sealed class IncidenceService(IRepository<Incidence> incidenceRepository, IReadOnlyRepository<TypeIncidence> incidenceReadOnlyTypeRepository)
{
    private readonly IRepository<Incidence> _incidenceRepository = incidenceRepository;
    private readonly IReadOnlyRepository<TypeIncidence> _typeIncidenceRepository = incidenceReadOnlyTypeRepository;

    public Guid Create(IncidenceArgs incidenceArgs)
    {
        Incidence incidence = new Incidence
        {
            Type = FindTypeIncidenceById(incidenceArgs.TypeIncidence),
            Description = incidenceArgs.Description,
            Start = incidenceArgs.Start,
            End = incidenceArgs.End,
            AttractionId = incidenceArgs.AttractionId,
            Active = incidenceArgs.Active
        };
        _incidenceRepository.Add(incidence);

        return incidence.Id;
    }

    public Incidence MapToEntity(IncidenceArgs incidenceArgs)
    {
        return new Incidence
        {
            Type = FindTypeIncidenceById(incidenceArgs.TypeIncidence),
            Description = incidenceArgs.Description,
            Start = incidenceArgs.Start,
            End = incidenceArgs.End,
            AttractionId = incidenceArgs.AttractionId,
            Active = incidenceArgs.Active
        };
    }

    public TypeIncidence? FindTypeIncidenceById(Guid typeIncidenceId)
    {
        return _typeIncidenceRepository.Get(t => t.Id == typeIncidenceId);
    }
}
