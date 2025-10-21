using FluentAssertions;
using VirtualPark.BusinessLogic.ViaAccess.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
namespace VirtualPark.BusinessLogic.Test.ViaAccess.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Nfc")]
public sealed class NfcTest
{
    #region Visitor
    [TestMethod]
    [TestCategory("Behaviour")]
    public void IdentifyVisitor_WhenCreatedWithVisitor_ShouldReturnSameVisitor()
    {
        var visitor = new VisitorProfile();

        var nfc = new Nfc(visitor);

        nfc.IdentifyVisitor().Should().Be(visitor);
    }
    #endregion

    #region NfcId

    [TestMethod]
    [TestCategory("Behaviour")]
    public void NfcId_WhenCreatedWithVisitor_ShouldMatchVisitorsNfcId()
    {
        var visitor = new VisitorProfile();

        var nfc = new Nfc(visitor);

        nfc.NfcId.Should().Be(visitor.NfcId);
    }
    #endregion
}
