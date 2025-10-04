using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public sealed class IncidenceService(IRepository<Incidence> incidenceRepository, IReadOnlyRepository<TypeIncidence> incidenceReadOnlyTypeRepository,
    IReadOnlyRepository<Attraction> attractionReadOnlyRepository)
{
    private readonly IRepository<Incidence> _incidenceRepository = incidenceRepository;
    private readonly IReadOnlyRepository<TypeIncidence> _typeIncidenceRepository = incidenceReadOnlyTypeRepository;
    private readonly IReadOnlyRepository<Attraction> _attractionRepository = attractionReadOnlyRepository;

    public Guid Create(IncidenceArgs incidenceArgs)
    {
        Incidence incidence = MapToEntity(incidenceArgs);
        _incidenceRepository.Add(incidence);

        return incidence.Id;
    }

    public List<Incidence> GetAll()
    {
        return _incidenceRepository.GetAll();
    }

    public Incidence Get(Guid id)
    {
        var incidence = incidenceRepository.Get(i => i.Id == id);

        if(incidence == null)
        {
            throw new InvalidOperationException("Incidence don't exist");
        }

        return incidence;
    }

    public bool Exist(Guid id)
    {
        return _incidenceRepository.Exist(i => i.Id == id);
    }

    public void Update(Guid id, IncidenceArgs incidenceArgs)
    {
        Incidence incidence = Get(id) ?? throw new InvalidOperationException($"Incidence with id {id} not found.");
        ApplyArgsToEntity(incidence, incidenceArgs);
        _incidenceRepository.Update(incidence);
    }

    public void Remove(Guid id)
    {
        Incidence incidence = Get(id) ?? throw new InvalidOperationException($"Incidence with id {id} not found.");
        _incidenceRepository.Remove(incidence);
    }

    public Incidence MapToEntity(IncidenceArgs incidenceArgs)
    {
        return new Incidence
        {
            Type = FindTypeIncidenceById(incidenceArgs.TypeIncidence),
            TypeIncidenceId = incidenceArgs.TypeIncidence,
            Description = incidenceArgs.Description,
            Start = incidenceArgs.Start,
            End = incidenceArgs.End,
            Attraction = FindTAttractionById(incidenceArgs.AttractionId),
            AttractionId = incidenceArgs.AttractionId,
            Active = incidenceArgs.Active
        };
    }

    public TypeIncidence? FindTypeIncidenceById(Guid typeIncidenceId)
    {
        return _typeIncidenceRepository.Get(t => t.Id == typeIncidenceId);
    }

    private Attraction? FindTAttractionById(Guid attractionId)
    {
        return _attractionRepository.Get(a => a.Id == attractionId);
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
