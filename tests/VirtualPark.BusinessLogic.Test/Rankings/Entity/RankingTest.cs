using FluentAssertions;
using VirtualPark.BusinessLogic.Rankings;
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
    #region Date
    [TestMethod]
    public void Date_GetSet_Works()
    {
        var ranking = new Ranking { Date = new DateTime(2000, 1, 1) };
        ranking.Date.Should().Be(new DateTime(2000, 1, 1));
    }

    #endregion
    #region Ranking

    [TestMethod]
    public void WhenRankingIsCreated_ListEntriesIsAssigned()
    {
        var ranking = new Ranking();
        ranking.Entries.Should().NotBeNull();
    }

    [TestMethod]
    public void WhenRankingIsCreated_ListEntriesIsEmpty()
    {
        var ranking = new Ranking();
        ranking.Entries.Should().BeEmpty();
    }
    #endregion
    #region Period

    [TestMethod]
    public void Period_GetSet_Works()
    {
        var ranking = new Ranking { Period = Period.Daily };
        ranking.Period.Should().Be(Period.Daily);
    }
    #endregion
}
