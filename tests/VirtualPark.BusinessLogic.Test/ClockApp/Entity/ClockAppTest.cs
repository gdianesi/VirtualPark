using VirtualPark.BusinessLogic.Clock.Entity;

namespace VirtualPark.BusinessLogic.Test.Clock.Entity;

[TestClass]
[TestCategory("ClockApp")]
public class ClockAppTest
{
    #region Id
    [TestMethod]
    public void Id_WhenClockIsCreated_ShouldBeGeneratedAutomatically()
    {
        var clock = new ClockApp();
        clock.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
}
