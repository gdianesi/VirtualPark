using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.ViaAccess.Entity;

public class Nfc : IViaAccess
{
    private readonly VisitorProfile _visitor;

    public Nfc(VisitorProfile visitor)
    {
        _visitor = visitor;
        NfcId = _visitor.NfcId;
    }

    public Guid NfcId { get; }

    public VisitorProfile IdentifyVisitor()
    {
        return _visitor;
    }
}
