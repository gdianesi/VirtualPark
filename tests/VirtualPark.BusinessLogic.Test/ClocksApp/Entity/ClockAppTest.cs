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

    #region DateSystem

    [TestMethod]
    public void DateSystem_GetAfterSet_ShouldReturnSameValue()
    {
        var expected = new DateTime(2025, 10, 3, 15, 30, 0);
        var clock = new ClockApp { DateSystem = expected };
        clock.DateSystem.Should().Be(expected);
    }

    [TestMethod]
    public void DateSystem_Create_ShouldBeCloseTo_Now()
    {
        var clock = new ClockApp();

        clock.DateSystem.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(5));
    }

    #endregion
}
