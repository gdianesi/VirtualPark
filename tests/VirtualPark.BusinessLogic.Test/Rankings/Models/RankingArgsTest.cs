using FluentAssertions;
using VirtualPark.BusinessLogic.Rankings;
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
        var rankingArgs = new RankingArgs("2025-09-27 00:00", [], "Daily");
        rankingArgs.Date.Should().Be(expected);
    }
    #endregion Date

    #region Users
    [TestMethod]
    public void Entries_InitSetter_SetsValue_OnInitialization()
    {
        List<Guid> expected = [Guid.NewGuid(), Guid.NewGuid()];

        var rankingArgs = new RankingArgs("2025-09-27 00:00", [], "Daily")
        {
            Entries = expected
        };

        rankingArgs.Entries.Should().BeSameAs(expected);
        rankingArgs.Entries.Should().HaveCount(2);
    }

    [TestMethod]
    public void Entries_InitSetter_OverridesCtorDefault()
    {
        List<string> ctorEntries = [Guid.NewGuid().ToString()];
        List<Guid> expected = [Guid.NewGuid()];

        var rankingArgs = new RankingArgs("2025-09-27 00:00", ctorEntries, "Daily")
        {
            Entries = expected
        };

        rankingArgs.Entries.Should().BeSameAs(expected);
        rankingArgs.Entries.Should().ContainSingle().Which.Should().Be(expected[0]);
    }
    #endregion Users

    #region Period
    [TestMethod]
    public void Period_Getter_ReturnsAssignedValue()
    {
        var rankingArgs = new RankingArgs("2025-09-27 00:00", [], "Daily");
        rankingArgs.Period.Should().Be(Period.Daily);
    }
    #endregion
}
