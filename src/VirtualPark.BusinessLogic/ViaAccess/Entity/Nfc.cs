using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.ViaAccess.Entity;

public class Nfc : IViaAccess
{
    private readonly Visitor _visitor;

    public Nfc(Visitor visitor)
    {
        _visitor = visitor;
        NfcId = _visitor.NfcId;
    }

    public Guid NfcId { get; set; }

    public Visitor IdentifyVisitor()
    {
        return _visitor;
    }
}
