using FluentAssertions;
using VirtualPark.WebApi.Controllers.Ranking.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Ranking.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("GetRankingRequest")]
public sealed class GetRankingRequestTest
{
    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingRequest_DateProperty_ShouldReturnAssignedValue()
    {
        var request = new GetRankingRequest { Date = "2025-10-06" };
        request.Date.Should().Be("2025-10-06");
    }
    #endregion
}
