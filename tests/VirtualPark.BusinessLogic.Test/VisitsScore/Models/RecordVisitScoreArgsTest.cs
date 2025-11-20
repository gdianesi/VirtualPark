using FluentAssertions;
using VirtualPark.BusinessLogic.VisitsScore.Models;

namespace VirtualPark.BusinessLogic.Test.VisitsScore.Models;

[TestClass]
[TestCategory("Args")]
[TestCategory("RecordVisitScoreArgs")]
public sealed class RecordVisitScoreArgsTest
{
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_ShouldParseVisitRegistrationId_TrimOrigin_AndAllowNullPoints()
    {
        var vrId = Guid.NewGuid();

        var args = new RecordVisitScoreArgs(
            visitRegistrationId: vrId.ToString(),
            origin: "  Atracción  ",
            points: null);

        args.VisitRegistrationId.Should().Be(vrId);
        args.Origin.Should().Be("Atracción");
        args.Points.Should().BeNull();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Constructor_ShouldParsePoints_WhenProvided()
    {
        var vr = Guid.NewGuid().ToString();

        var a1 = new RecordVisitScoreArgs(vr, "Canje", "-30");
        var a2 = new RecordVisitScoreArgs(vr, "Atracción", "0");
        var a3 = new RecordVisitScoreArgs(vr, "Atracción", null);

        a1.Points.Should().Be(-30);
        a2.Points.Should().Be(0);
        a3.Points.Should().BeNull();
    }
}
