using FluentAssertions;
using VirtualPark.BusinessLogic.Rankings.Models;

namespace VirtualPark.BusinessLogic.Test.Rankings.Models;

[TestClass]
[TestCategory("RankingArgs")]
public class RankingArgsTest
{
    #region Date

    [TestMethod]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var expected = new DateTime(2025, 9, 27, 00, 00, 00);
        var rankingArgs = new RankingArgs("2025-09-27 00:00", Array.Empty<string>());
        rankingArgs.Date.Should().Be(expected);
    }
    #endregion Date
    #region Users

    [TestMethod]
    public void Entries_Getter_ReturnsValueAfterSet()
    {
        var rankingArgs = new RankingArgs("2025-09-27 00:00", Array.Empty<string>());
        var expected = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        rankingArgs.Entries = expected;

        rankingArgs.Entries.Should().BeSameAs(expected);
        rankingArgs.Entries.Should().HaveCount(2);
    }

    [TestMethod]
    public void Entries_Setter_ReplacesPreviousList()
    {
        var rankingArgs = new RankingArgs("2025-09-27 00:00", Array.Empty<string>());
        var first = new List<Guid> { Guid.NewGuid() };
        var second = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        rankingArgs.Entries = first;
        rankingArgs.Entries = second;

        rankingArgs.Entries.Should().BeSameAs(second);
        rankingArgs.Entries.Should().NotBeSameAs(first);
        rankingArgs.Entries.Should().HaveCount(3);
    }

    #endregion Users

}
