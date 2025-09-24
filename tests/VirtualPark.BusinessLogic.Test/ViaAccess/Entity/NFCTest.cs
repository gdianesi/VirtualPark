using FluentAssertions;
using VirtualPark.BusinessLogic.ViaAccess.Entity;
using VirtualPark.BusinessLogic.Visitors.Entity;
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
        var visitor = new Visitor { Name = "John Doe" };

        var nfc = new Nfc(visitor);

        nfc.IdentifyVisitor().Should().Be(visitor);
    }
    #endregion
}
