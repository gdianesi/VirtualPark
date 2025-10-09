using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public sealed class IncidenceService(IRepository<Incidence> incidenceRepository, IReadOnlyRepository<TypeIncidence> incidenceReadOnlyTypeRepository,
    IReadOnlyRepository<Attraction> attractionReadOnlyRepository) : IIncidenceService
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
        var incidence = _incidenceRepository.Get(
            i => i.Id == id,
            include: q => q
                .Include(i => i.Type)
                .Include(i => i.Attraction));

        if(incidence == null)
        {
            throw new InvalidOperationException("Incidence don't exist");
        }

        return incidence;
    }

    public void Update(IncidenceArgs incidenceArgs, Guid id)
    {
        Incidence incidence = Get(id);
        ApplyArgsToEntity(incidence, incidenceArgs);
        _incidenceRepository.Update(incidence);
    }

    public void Remove(Guid id)
    {
        Incidence incidence = Get(id);
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
        var type = _typeIncidenceRepository.Get(t => t.Id == typeIncidenceId);

        if(type == null)
        {
            throw new InvalidOperationException("Type incidence don't exist");
        }

        return type;
    }

    private Attraction? FindTAttractionById(Guid attractionId)
    {
        var attraction = _attractionRepository.Get(a => a.Id == attractionId);

        if(attraction == null)
        {
            throw new InvalidOperationException("Attraction don't exist");
        }

        return attraction;
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
