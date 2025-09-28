using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Service;

public class VisitorProfileService(IRepository<VisitorProfile> visitorProfileRepository) : IVisitorProfile
{
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;

    public VisitorProfile Create(VisitorProfileArgs args)
    {
        var entity = MapToEntity(args);

        _visitorProfileRepository.Add(entity);

        return entity;
    }

    private static VisitorProfile MapToEntity(VisitorProfileArgs args) => new VisitorProfile
    {
        DateOfBirth = args.DateOfBirth,
        Membership = args.Membership
    };
}
