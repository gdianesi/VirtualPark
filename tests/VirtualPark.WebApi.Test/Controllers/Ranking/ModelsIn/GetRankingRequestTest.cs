using FluentAssertions;
using VirtualPark.WebApi.Controllers.Ranking.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Ranking.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("GetRankingRequest")]
public sealed class GetRankingRequestTest
{
    private static GetRankingRequest Build(string? date = null, string? period = null)
    {
        return new GetRankingRequest
        {
            Date = date ?? "2025-10-06",
            Period = period ?? "Weekly"
        };
    }

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingRequest_DateProperty_ShouldReturnAssignedValue()
    {
        var request = Build(date: "2025-10-06");
        request.Date.Should().Be("2025-10-06");
    }
    #endregion

    #region Period
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingRequest_PeriodProperty_ShouldMatchCtorValue()
    {
        var request = Build(period: "Weekly");
        request.Period.Should().Be("Weekly");
    }
    #endregion

    #region ToArgs
    [TestMethod]
    [TestCategory("Conversion")]
    public void ToArgs_WhenValid_ShouldReturnRankingArgs()
    {
        var request = Build(date: "2025-10-06", period: "Daily");
        var result = request.ToArgs();

        result.Date.Should().Be(DateTime.Parse("2025-10-06"));
        result.Period.ToString().Should().Be("Daily");
    }
    #endregion
}
