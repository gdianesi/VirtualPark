using FluentAssertions;
using VirtualPark.BusinessLogic.ClocksApp.Models;

namespace VirtualPark.BusinessLogic.Test.ClocksApp.Models;

[TestClass]
[TestCategory("ClocksAppArgs")]
public class ClockAppArgsTest
{
    #region SystemDateTime

    [TestMethod]
    public void SystemDateTime_ShouldParseStringDateTime_ToDateTimeProperty()
    {
        var clockArgs = new ClockAppArgs("2025-10-02 18:30");
        clockArgs.SystemDateTime.Should().Be(new DateTime(2025, 10, 02, 18, 30, 0));
    }
    #endregion

}
