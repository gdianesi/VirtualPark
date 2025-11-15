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
        List<string>? scores = null,
        string? period = null)
    {
        return new GetRankingResponse(
            id: id ?? Guid.NewGuid().ToString(),
            date: date ?? "2025-10-06",
            users: users ?? [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()],
            scores: scores ?? ["10", "20"],
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

    #region Scores
    [TestMethod]
    [TestCategory("Validation")]
    public void GetRankingResponse_ScoresProperty_ShouldMatchCtorValue()
    {
        const string s1 = "50";
        const string s2 = "75";

        var response = Build(scores: [s1, s2]);

        response.Scores.Should().NotBeNull();
        response.Scores.Should().BeEquivalentTo([s1, s2]);
    }
    #endregion
}
