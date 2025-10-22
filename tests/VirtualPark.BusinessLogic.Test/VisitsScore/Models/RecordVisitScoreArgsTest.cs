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
    #endregion
}
