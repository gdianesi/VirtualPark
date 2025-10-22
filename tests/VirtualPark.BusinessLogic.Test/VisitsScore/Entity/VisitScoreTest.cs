using FluentAssertions;
using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.BusinessLogic.Test.VisitsScore.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitScore")]
public class VisitScoreTest
{
    #region Id
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitScoreIsCreated_ShouldAssignId()
    {
        var score = new VisitScore();
        score.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Origin
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void Origin_Getter_ShouldReturnAssignedInstance()
    {
        var score = new VisitScore { Origin = "Atracción" };

        score.Origin.Should().Be("Atracción");
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Setter")]
    public void Origin_Setter_ShouldStoreAssignedInstance()
    {
        var score = new VisitScore();
        score.Origin = "Canje";

        score.Origin.Should().Be("Canje");
    }
    #endregion
    #endregion

    #region OccurredAt
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void OccurredAt_Getter_ShouldReturnAssignedInstance()
    {
        var when = new DateTime(2025, 9, 2, 10, 30, 00, DateTimeKind.Utc);
        var score = new VisitScore { OccurredAt = when };

        score.OccurredAt.Should().Be(when);
    }
    #endregion
    #endregion
}
