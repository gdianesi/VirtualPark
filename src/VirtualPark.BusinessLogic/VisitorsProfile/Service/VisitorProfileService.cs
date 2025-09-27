using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Service;

public class VisitorProfileService(IRepository<VisitorProfile> visitorProfileRepository)
{
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;

    public Guid Create(VisitorProfileArgs args)
    {
        var entity = MapToEntity(args);
        _visitorProfileRepository.Add(entity);
        return entity.Id;
    }

    private static VisitorProfile MapToEntity(VisitorProfileArgs args) => new VisitorProfile
    {
        Id = Guid.NewGuid(),
        DateOfBirth = args.DateOfBirth,
        Membership = args.Membership
    };
}
