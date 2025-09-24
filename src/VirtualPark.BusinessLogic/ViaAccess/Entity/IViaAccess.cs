using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.ViaAccess.Entity;

public interface IViaAccess
{
    Visitor IdentifyVisitor();
}
