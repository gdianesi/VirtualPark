using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Service;

public interface IVisitorProfile
{
    public VisitorProfile Create(VisitorProfileArgs args);
}
