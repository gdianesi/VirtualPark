using FluentAssertions;
using VirtualPark.BusinessLogic.VisitsScore.Models;

namespace VirtualPark.BusinessLogic.Test.VisitsScore.Models;

[TestClass]
[TestCategory("Model")]
[TestCategory("RecordVisitScoreArgs")]
public class RecordVisitScoreArgsTest
{
    #region VisitorProfileId
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void VisitorProfileId_Getter_ShouldReturnAssignedInstance()
    {
        var id = Guid.NewGuid();
        var args = new RecordVisitScoreArgs { VisitorProfileId = id };

        args.VisitorProfileId.Should().Be(id);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Setter")]
    public void VisitorProfileId_Setter_ShouldStoreAssignedInstance()
    {
        var id = Guid.NewGuid();
        var args = new RecordVisitScoreArgs();

        args.VisitorProfileId = id;

        args.VisitorProfileId.Should().Be(id);
    }
    #endregion
    #endregion

    #region Origin
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void Origin_Getter_ShouldReturnAssignedInstance()
    {
        var args = new RecordVisitScoreArgs { Origin = "Atracción" };

        args.Origin.Should().Be("Atracción");
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Setter")]
    public void Origin_Setter_ShouldStoreAssignedInstance()
    {
        var args = new RecordVisitScoreArgs();

        args.Origin = "Canje";

        args.Origin.Should().Be("Canje");
    }
    #endregion
    #endregion
}
