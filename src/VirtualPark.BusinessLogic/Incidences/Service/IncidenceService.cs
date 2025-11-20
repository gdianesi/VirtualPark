using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Incidences.Service;

public sealed class IncidenceService(IRepository<Incidence> incidenceRepository, IReadOnlyRepository<TypeIncidence> incidenceReadOnlyTypeRepository,
    IReadOnlyRepository<Attraction> attractionReadOnlyRepository, IClockAppService clockAppService) : IIncidenceService
{
    private readonly IRepository<Incidence> _incidenceRepository = incidenceRepository;
    private readonly IReadOnlyRepository<TypeIncidence> _typeIncidenceRepository = incidenceReadOnlyTypeRepository;
    private readonly IReadOnlyRepository<Attraction> _attractionRepository = attractionReadOnlyRepository;
    private readonly IClockAppService _clockAppService = clockAppService;

    public Guid Create(IncidenceArgs incidenceArgs)
    {
        Incidence incidence = MapToEntity(incidenceArgs);
        _incidenceRepository.Add(incidence);

        return incidence.Id;
    }

    public List<Incidence> GetAll()
    {
        var allIds = _incidenceRepository
            .GetAll()
            .Select(i => i.Id)
            .ToList();

        var now = _clockAppService.Now();

        var incidences = allIds.Select(id =>
                _incidenceRepository.Get(
                    i => i.Id == id,
                    include: q => q.Include(i => i.Type)
                        .Include(i => i.Attraction)))
            .OfType<Incidence>()
            .ToList();

        foreach(var inc in incidences)
        {
            AutoActivateIfValid(inc, now);

            AutoDeactivateIfExpired(inc, now);
        }

        return incidences;
    }

    public Incidence Get(Guid id)
    {
        var incidence = _incidenceRepository.Get(
            i => i.Id == id,
            include: q => q.Include(i => i.Type)
                .Include(i => i.Attraction)) ?? throw new InvalidOperationException("Incidence don't exist");
        var now = _clockAppService.Now();
        AutoActivateIfValid(incidence, now);
        AutoDeactivateIfExpired(incidence, now);

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
            Active = incidenceArgs.Active,
            ManualOverride = false
        };
    }

    public TypeIncidence? FindTypeIncidenceById(Guid typeIncidenceId)
    {
        var type = _typeIncidenceRepository.Get(t => t.Id == typeIncidenceId) ?? throw new InvalidOperationException("Type incidence don't exist");
        return type;
    }

    private Attraction? FindTAttractionById(Guid attractionId)
    {
        var attraction = _attractionRepository.Get(a => a.Id == attractionId) ?? throw new InvalidOperationException("Attraction don't exist");
        return attraction;
    }

    public void ApplyArgsToEntity(Incidence entity, IncidenceArgs args)
    {
        entity.Type = FindTypeIncidenceById(args.TypeIncidence);
        entity.Description = args.Description;
        entity.Start = args.Start;
        entity.End = args.End;
        entity.AttractionId = args.AttractionId;

        if(entity.Active != args.Active)
        {
            entity.ManualOverride = true;
        }

        entity.Active = args.Active;
    }

    public bool HasActiveIncidenceForAttraction(Guid attractionId, DateTime dateTime)
    {
        var incidences = _incidenceRepository.GetAll(
            i => i.AttractionId == attractionId && i.Active);

        var hasActive = false;

        foreach(var inc in incidences.Where(inc => !AutoDeactivateIfExpired(inc, dateTime)).
                     Where(inc => inc.Start <= dateTime && inc.End >= dateTime))
        {
            hasActive = true;
        }

        return hasActive;
    }

    private bool AutoDeactivateIfExpired(Incidence incidence, DateTime now)
    {
        if(incidence.End < now)
        {
            if(incidence.Active)
            {
                incidence.Active = false;
                _incidenceRepository.Update(incidence);
            }

            return true;
        }

        if(incidence.ManualOverride)
        {
            return false;
        }

        if(!incidence.Active || incidence.End >= now)
        {
            return false;
        }

        incidence.Active = false;
        _incidenceRepository.Update(incidence);
        return true;
    }

    private bool AutoActivateIfValid(Incidence incidence, DateTime now)
    {
        if(incidence.ManualOverride)
        {
            return false;
        }

        if(incidence.Active)
        {
            return false;
        }

        if(incidence.Start <= now && now <= incidence.End)
        {
            incidence.Active = true;
            _incidenceRepository.Update(incidence);
            return true;
        }

        return false;
    }
}
