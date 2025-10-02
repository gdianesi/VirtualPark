using FluentAssertions;

namespace VirtualPark.BusinessLogic.Test.ClockApp.Entity;

[TestClass]
[TestCategory("ClockApp")]
public class ClockAppTest
{
    #region Id
    [TestMethod]
    public void Id_WhenClockIsCreated_ShouldBeGeneratedAutomatically()
    {
        var clock = new BusinessLogic.ClockApp.Entity.ClockApp();
        clock.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
}
