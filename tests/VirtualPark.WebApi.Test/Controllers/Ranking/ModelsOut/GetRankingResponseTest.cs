using FluentAssertions;
using VirtualPark.BusinessLogic.Rankings;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Ranking.ModelsOut;

[TestClass]
[TestCategory("GetRankingResponse")]
public sealed class GetRankingResponseTest
{
    private static BusinessLogic.Rankings.Entity.Ranking BuildEntity(
        Guid? id = null,
        DateTime? date = null,
        List<(Guid UserId, int Score)>? entries = null,
        Period? period = null)
    {
        return new BusinessLogic.Rankings.Entity.Ranking
        {
            Id = id ?? Guid.NewGuid(),
            Date = date ?? new DateTime(2025, 10, 06),
            Period = period ?? Period.Daily,
            Entries = (entries ??
            [
                (Guid.NewGuid(), 10),
                (Guid.NewGuid(), 20)
            ])
            .Select(e => new User
            {
                Id = e.UserId,
                VisitorProfile = new VisitorProfile { Score = e.Score }
            })
            .ToList()
        };
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetRankingResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Date
    [TestMethod]
    public void Date_ShouldMapCorrectly()
    {
        var date = new DateTime(2026, 01, 15);
        var entity = BuildEntity(date: date);

        var dto = new GetRankingResponse(entity);

        dto.Date.Should().Be("2026-01-15");
    }
    #endregion

    #region Users
    [TestMethod]
    public void Users_ShouldMapCorrectly()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();

        var entity = BuildEntity(entries:
        [
            (u1, 50),
            (u2, 75)
        ]);

        var dto = new GetRankingResponse(entity);

        dto.Users.Should().BeEquivalentTo([u1.ToString(), u2.ToString()]);
    }
    #endregion

    #region Scores
    [TestMethod]
    public void Scores_ShouldMapCorrectly()
    {
        var entity = BuildEntity(entries:
        [
            (Guid.NewGuid(), 50),
            (Guid.NewGuid(), 75)
        ]);

        var dto = new GetRankingResponse(entity);

        dto.Scores.Should().BeEquivalentTo(["50", "75"]);
    }
    #endregion

    #region Period
    [TestMethod]
    public void Period_ShouldMapCorrectly()
    {
        var entity = BuildEntity(period: Period.Monthly);

        var dto = new GetRankingResponse(entity);

        dto.Period.Should().Be("Monthly");
    }
    #endregion
}
