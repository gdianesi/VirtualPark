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
}
