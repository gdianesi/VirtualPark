using FluentAssertions;
using VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Ranking.ModelsOut;

[TestClass]
[TestCategory("GetRankingResponse")]
public sealed class GetRankingResponseTest
{
    private static GetRankingResponse Build(
        string? id = null,
        string? date = null,
        List<string>? users = null,
        string? period = null)
    {
        return new GetRankingResponse(
            id: id ?? Guid.NewGuid().ToString(),
            date: date ?? "2025-10-06",
        users: users ?? [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()],
            period: period ?? "Daily");
    }

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingResponse_IdProperty_ShouldMatchCtorValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = Build(id: id);
        response.Id.Should().Be(id);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingResponse_DateProperty_ShouldMatchCtorValue()
    {
        var response = Build(date: "2025-10-06");
        response.Date.Should().Be("2025-10-06");
    }
    #endregion

    #region Users
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingResponse_UsersProperty_ShouldMatchCtorValue()
    {
        var u1 = Guid.NewGuid().ToString();
        var u2 = Guid.NewGuid().ToString();

        var response = Build(users: [u1, u2]);

        response.Users.Should().NotBeNull();
        response.Users.Should().BeEquivalentTo([u1, u2]);
    }
    #endregion

    #region Period
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingResponse_PeriodProperty_ShouldMatchCtorValue()
    {
        var response = Build(period: "Weekly");
        response.Period.Should().Be("Weekly");
    }
    #endregion
}
