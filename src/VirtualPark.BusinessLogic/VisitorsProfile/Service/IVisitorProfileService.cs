using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Service;

public interface IVisitorProfileService
{
    public VisitorProfile Create(VisitorProfileArgs args);
    public void Remove(Guid? id);
    public void Update(VisitorProfileArgs visitorProfileArgs, Guid visitorId);
    public VisitorProfile? Get(Guid id);
    public List<VisitorProfile> GetAll();
}
