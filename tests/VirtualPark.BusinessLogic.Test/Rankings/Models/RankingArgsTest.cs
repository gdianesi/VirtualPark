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
        var rankingArgs = new RankingArgs("2025-09-27 00:00", "Daily");
        rankingArgs.Date.Should().Be(expected);
    }
    #endregion Date

    #region Period
    [TestMethod]
    public void Period_Getter_ReturnsAssignedValue()
    {
        var rankingArgs = new RankingArgs("2025-09-27 00:00", "Daily");
        rankingArgs.Period.Should().Be(Period.Daily);
    }
    #endregion
}
