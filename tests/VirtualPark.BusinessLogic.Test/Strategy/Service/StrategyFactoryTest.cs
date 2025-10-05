using FluentAssertions;
using VirtualPark.BusinessLogic.Strategy.Services;

namespace VirtualPark.BusinessLogic.Test.Strategy.Service;

[TestClass]
public class StrategyFactoryTest
{
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_ShouldReturnNull_WhenKeyIsAnyNonEmptyString()
    {
        var factory = new StrategyFactory();
        var key = "any-key";

        var result = factory.Create(key);

        result.Should().BeNull();
    }
}
