using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.TypeIncidences.Service;

public sealed class TypeIncidenceService(IRepository<TypeIncidence> typeIncidenceRepository)
{
    private readonly IRepository<TypeIncidence> _typeIncidenceRepository = typeIncidenceRepository;

    public Guid Create(TypeIncidenceArgs args)
    {
        TypeIncidence typeIncidence = MapToEntity(args);
        _typeIncidenceRepository.Add(typeIncidence);
        return typeIncidence.Id;
    }

    public List<TypeIncidence> GetAll(Expression<Func<TypeIncidence, bool>>? predicate = null)
    {
        return (predicate == null) ? _typeIncidenceRepository.GetAll() : _typeIncidenceRepository.GetAll(predicate);
    }

    public TypeIncidence MapToEntity(TypeIncidenceArgs args)
    {
        return new TypeIncidence
        {
            Type = args.Type
        };
    }
}
