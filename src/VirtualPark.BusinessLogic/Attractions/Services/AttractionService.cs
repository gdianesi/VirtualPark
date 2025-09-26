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

    public void ValidateAttractionName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Attraction name cannot be empty.", nameof(name));
        }

        if (_attractionRepository.Exist(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
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
