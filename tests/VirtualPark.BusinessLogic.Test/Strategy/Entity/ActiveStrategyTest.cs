using FluentAssertions;
using VirtualPark.BusinessLogic.Strategy.Entity;

namespace VirtualPark.BusinessLogic.Test.Strategy.Entity;

[TestClass]
[TestCategory("ActiveStrategy")]
[TestCategory("Entity")]
public class ActiveStrategyTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void WhenActiveStrategyIsCreated_IdIsAssigned()
    {
        var activeStrategy = new ActiveStrategy();
        activeStrategy.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
    #region StrategyKey

    [TestMethod]
    [TestCategory("Validation")]
    public void StrategyKey_GetSet_Works()
    {
        var activeStrategy = new ActiveStrategy { StrategyKey = "byAttraction" };
        activeStrategy.StrategyKey.Should().Be("byAttraction");
    }
    #endregion
    #region Date

    [TestMethod]
    [TestCategory("Validation")]
    public void Date_GetSet_Works()
    {
        var activeStrategy = new ActiveStrategy { Date = new DateTime(2000, 1, 1) };
        activeStrategy.Date.Should().Be(new DateTime(2000, 1, 1));
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_ShouldInitializeDateWithCurrentTime()
    {
        var activeStrategy = new ActiveStrategy();
        activeStrategy.Date.Should().Be(DateTime.Today);
    }
    #endregion
}
