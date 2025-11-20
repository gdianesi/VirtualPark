using FluentAssertions;
using VirtualPark.BusinessLogic.Strategy.Models;

namespace VirtualPark.BusinessLogic.Test.Strategy.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("StrategyArgs")]
public sealed class StrategyArgsTest
{
    #region Key
    [TestMethod]
    public void Constructor_ShouldAssignKey_WhenValid()
    {
        var args = new StrategyArgs("Attraction");
        args.Key.Should().Be("Attraction");
    }
    #endregion
}
