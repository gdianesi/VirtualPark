using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Attractions.Services;

public sealed class AttractionService(IRepository<Attraction> attractionRepository)
{
    private readonly IRepository<Attraction> _attractionRepository = attractionRepository;

    public Attraction Create(AttractionArgs args)
    {
        var attraction = MapToEntity(args);

        _attractionRepository.Add(attraction);

        return attraction;
    }

    public List<Attraction> GetAll(Expression<Func<Attraction, bool>>? predicate = null)
    {
        if(predicate == null)
        {
            return _attractionRepository.GetAll();
        }

        return _attractionRepository.GetAll(predicate);
    }

    public Attraction? Get(Expression<Func<Attraction, bool>> predicate)
    {
        return _attractionRepository.Get(predicate);
    }

    public bool Exist(Expression<Func<Attraction, bool>> predicate)
    {
        return _attractionRepository.Exist(predicate);
    }

    public void Update(AttractionArgs args, Guid id)
    {
        Attraction attraction = Get(a => a.Id == id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        ApplyArgsToEntity(attraction, args);

        _attractionRepository.Update(attraction);
    }

    public void Remove(Guid id)
    {
        Attraction attraction = Get(a => a.Id == id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        _attractionRepository.Remove(attraction);
    }

    public static void ApplyArgsToEntity(Attraction entity, AttractionArgs args)
    {
        entity.Type = args.Type;
        entity.Name = args.Name;
        entity.MiniumAge = args.MiniumAge;
        entity.Capacity = args.Capacity;
        entity.Description = args.Description;
        entity.CurrentVisitors = args.CurrentVisitor;
        entity.Available = args.Available;
    }

    public void ValidateAttractionName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Attraction name cannot be empty.", nameof(name));
        }

        if(_attractionRepository.Exist(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
        {
            throw new Exception("Attraction name already exists.");
        }
    }

    public Attraction MapToEntity(AttractionArgs args)
    {
        ValidateAttractionName(args.Name);
        ValidationServices.ValidateAge(args.MiniumAge);

        var attraction = new Attraction
        {
            Name = args.Name,
            Type = args.Type,
            Description = args.Description,
            MiniumAge = args.MiniumAge,
            Capacity = args.Capacity,
            CurrentVisitors = args.CurrentVisitor,
            Available = args.Available
        };

        return attraction;
    }
}
