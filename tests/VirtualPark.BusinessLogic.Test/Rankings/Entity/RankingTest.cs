using FluentAssertions;
using VirtualPark.BusinessLogic.Rankings.Entity;

namespace VirtualPark.BusinessLogic.Test.Rankings.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Ranking")]
public class RankingTest
{
    #region id
    [TestMethod]
    public void WhenRankingIsCreated_IdIsAssigned()
    {
        var ranking = new Ranking();
        ranking.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
}
