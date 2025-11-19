using FluentAssertions;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.VisitsScore.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetVisitScoreResponse")]
public class GetVisitScoreResponseTest
{
    private static VisitScore Build(
        Guid? id = null,
        string? origin = null,
        DateTime? occurredAt = null,
        int? points = null,
        string? strategy = null,
        Guid? regId = null)
    {
        return new VisitScore
        {
            Id = id ?? Guid.NewGuid(),
            Origin = origin ?? "Attraction",
            OccurredAt = occurredAt ?? new DateTime(2025, 10, 08, 12, 00, 00, DateTimeKind.Utc),
            Points = points ?? 42,
            DayStrategyName = strategy,
            VisitRegistrationId = regId ?? Guid.NewGuid(),
            VisitRegistration = new VisitRegistration()
        };
    }

    #region Id
    [TestMethod]
    public void Id_Getter_ReturnsValue()
    {
        var id = Guid.NewGuid();
        var score = Build(id: id);

        var resp = new GetVisitScoreResponse(score);

        resp.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Origin
    [TestMethod]
    public void Origin_Getter_ReturnsValue()
    {
        var score = Build(origin: "Event");

        var resp = new GetVisitScoreResponse(score);

        resp.Origin.Should().Be("Event");
    }
    #endregion

    #region OccurredAt
    [TestMethod]
    public void OccurredAt_Getter_ReturnsIso8601UtcFormat()
    {
        var dt = new DateTime(2025, 10, 08, 12, 00, 00, DateTimeKind.Utc);
        var score = Build(occurredAt: dt);

        var resp = new GetVisitScoreResponse(score);

        resp.OccurredAt.Should().Be(dt.ToUniversalTime().ToString("O"));
    }
    #endregion

    #region Points
    [TestMethod]
    public void Points_Getter_ReturnsValue()
    {
        var score = Build(points: 99);

        var resp = new GetVisitScoreResponse(score);

        resp.Points.Should().Be(99);
    }
    #endregion

    #region DayStrategyName
    [TestMethod]
    public void DayStrategyName_Getter_ReturnsValue()
    {
        var score = Build(strategy: "DailyStrategy");

        var resp = new GetVisitScoreResponse(score);

        resp.DayStrategyName.Should().Be("DailyStrategy");
    }

    [TestMethod]
    public void DayStrategyName_AllowsNull()
    {
        var score = Build(strategy: null);

        var resp = new GetVisitScoreResponse(score);

        resp.DayStrategyName.Should().BeNull();
    }
    #endregion

    #region VisitRegistrationId
    [TestMethod]
    public void VisitRegistrationId_Getter_ReturnsValue()
    {
        var regId = Guid.NewGuid();
        var score = Build(regId: regId);

        var resp = new GetVisitScoreResponse(score);

        resp.VisitRegistrationId.Should().Be(regId.ToString());
    }
    #endregion
}
