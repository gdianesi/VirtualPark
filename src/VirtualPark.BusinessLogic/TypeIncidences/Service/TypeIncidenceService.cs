using System.Linq.Expressions;
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

    public List<TypeIncidence> GetAll()
    {
        return _typeIncidenceRepository.GetAll();
    }

    public TypeIncidence Get(Guid id)
    {
        return _typeIncidenceRepository.Get(t => t.Id == id);
    }

    private bool Exist(Expression<Func<TypeIncidence, bool>> predicate)
    {
        return _typeIncidenceRepository.Exist(predicate);
    }

    public void Update(Guid id, TypeIncidenceArgs args)
    {
        TypeIncidence typeIncidence = Get(id) ?? throw new InvalidOperationException($"TypeIncidence with id {id} not found.");
        ApplyArgsToEntity(typeIncidence, args);
        _typeIncidenceRepository.Update(typeIncidence);
    }

    public void Delete(Guid id)
    {
        TypeIncidence typeIncidence = _typeIncidenceRepository.Get(t => t.Id == id) ?? throw new InvalidOperationException($"TypeIncidence with id {id} not found.");
        _typeIncidenceRepository.Remove(typeIncidence);
    }

    public static void ApplyArgsToEntity(TypeIncidence entity, TypeIncidenceArgs args)
    {
        entity.Type = args.Type;
    }

    public TypeIncidence MapToEntity(TypeIncidenceArgs args)
    {
        return new TypeIncidence
        {
            Type = args.Type
        };
    }
}
