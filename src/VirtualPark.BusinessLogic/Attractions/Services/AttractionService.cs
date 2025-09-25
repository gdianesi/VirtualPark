using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Attractions.Services;

public sealed class AttractionService(IRepository<Attraction> attractionRepository)
{
    private readonly IRepository<Attraction> _attractionRepository = attractionRepository;
}
