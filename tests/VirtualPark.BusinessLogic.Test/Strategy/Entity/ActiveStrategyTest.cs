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
}
