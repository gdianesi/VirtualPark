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

    public void Remove(Guid? id)
    {
        var visitorProfile = _visitorProfileRepository.Get(v => v.Id == id);

        if(visitorProfile == null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        _visitorProfileRepository.Remove(visitorProfile);
    }

    public VisitorProfile? Get(Guid id)
    {
        var visitorProfile = _visitorProfileRepository.Get(v => v.Id == id);

        if(visitorProfile == null)
        {
            throw new InvalidOperationException("Visitor don't exist");
        }

        return visitorProfile;
    }

    public void Update(VisitorProfileArgs args, Guid visitorId)
    {
        var visitorProfile = Get(visitorId);

        ApplyChange(visitorProfile!, args);

        _visitorProfileRepository.Update(visitorProfile!);
    }

    private void ApplyChange(VisitorProfile visitorProfile, VisitorProfileArgs args)
    {
        visitorProfile.Membership = args.Membership;
        visitorProfile.DateOfBirth = args.DateOfBirth;
        visitorProfile.Score = args.Score;
    }

    private static VisitorProfile MapToEntity(VisitorProfileArgs args) => new VisitorProfile
    {
        DateOfBirth = args.DateOfBirth,
        Membership = args.Membership
    };
}
