using FluentAssertions;
using VirtualPark.BusinessLogic.ClocksApp.Entity;

namespace VirtualPark.BusinessLogic.Test.ClocksApp.Entity;

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

    #region OffSetMinutes

    [TestMethod]
    public void OffsetMinutes_GetAfterSet_ShouldReturnSameValue()
    {
        var clock = new ClockApp{ OffsetMinutes = 100 };
        clock.OffsetMinutes.Should().Be(100);
    }
    #endregion
}
