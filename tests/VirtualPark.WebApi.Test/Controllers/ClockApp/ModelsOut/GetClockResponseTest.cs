using FluentAssertions;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.ClockApp.ModelsOut;

[TestClass]
[TestCategory("GetClockResponse")]
public sealed class GetClockResponseTest
{
    private static GetClockResponse Build(
        string? dateSystem = null)
    {
        return new GetClockResponse(
            dateSystem: dateSystem ?? "2025-10-06T22:00:00");
    }
    #region DateSystem
    [TestMethod]
    [TestCategory("Validation")]
    public void GetClockResponse_DateSystemProperty_ShouldMatchCtorValue()
    {
        var response = Build(dateSystem: "2025-10-06T21:45:00");
        response.DateSystem.Should().Be("2025-10-06T21:45:00");
    }
    #endregion
}
