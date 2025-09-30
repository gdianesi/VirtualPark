using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public class VisitRegistrationService(IRepository<VisitRegistration> visitRegistrationRepository, IReadOnlyRepository<VisitorProfile> visitorProfileRepository, IReadOnlyRepository<Attraction> attractionRepository)
{
    private readonly IRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    private readonly IReadOnlyRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IReadOnlyRepository<Attraction> _attractionRepository = attractionRepository;
    public VisitRegistration Create(VisitRegistrationArgs args)
    {
        var entity = MapToEntity(args);

        _visitRegistrationRepository.Add(entity);

        return entity;
    }

    private VisitRegistration MapToEntity(VisitRegistrationArgs args)
    {
        var visitor = _visitorProfileRepository.Get(v => v.Id == args.VisitorProfileId);
        if (visitor is null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        List<Attraction> attractions = new List<Attraction>();
        foreach(var attractionId in args.AttractionsId)
        {
            var attraction = _attractionRepository.Get(x => x.Id == attractionId);
            if(attraction is null)
            {
                throw new InvalidOperationException("Attraction don't exist");
            }

            attractions.Add(attraction);
        }

        return new VisitRegistration
        {
            VisitorId = visitor.Id,
            Visitor = visitor,
            Attractions = attractions
        };
    }
}
