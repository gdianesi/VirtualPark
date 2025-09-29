using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Attractions.Entity;
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
        Incidence incidence = MapToEntity(incidenceArgs);
        _incidenceRepository.Add(incidence);

        return incidence.Id;
    }

    public List<Incidence> GetAll(Expression<Func<Incidence, bool>>? predicate = null)
    {
        return _incidenceRepository.GetAll(predicate);
    }

    public Incidence? Get(Expression<Func<Incidence, bool>> predicate)
    {
        return incidenceRepository.Get(predicate);
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

    public void ApplyArgsToEntity(Incidence entity, IncidenceArgs args)
    {
        entity.Type = FindTypeIncidenceById(args.TypeIncidence);
        entity.Description = args.Description;
        entity.Start = args.Start;
        entity.End = args.End;
        entity.AttractionId = args.AttractionId;
        entity.Active = args.Active;
    }
}
