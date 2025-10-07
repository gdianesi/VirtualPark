using FluentAssertions;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.ClockApp.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("UpdateClockRequest")]
public sealed class UpdateClockRequestTest
{
    #region DateSystem
    [TestMethod]
    [TestCategory("Validation")]
    public void DateSystem_Getter_ShouldReturnAssignedValue()
    {
        var request = new UpdateClockRequest { DateSystem = "2025-10-06T21:45:00" };
        request.DateSystem.Should().Be("2025-10-06T21:45:00");
    }
    #endregion

    #region ToArgs
    [TestMethod]
    [TestCategory("Conversion")]
    public void ToArgs_WhenDateSystemIsValid_ShouldReturnClockAppArgs()
    {
        var request = new UpdateClockRequest { DateSystem = "2025-10-06T21:45:00" };
        var result = request.ToArgs();
        result.SystemDateTime.Should().Be(DateTime.Parse("2025-10-06T21:45:00"));
    }
    #endregion
}
