using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Service;
using VirtualPark.WebApi.Controllers.VisitsScore;

namespace VirtualPark.WebApi.Test.Controllers.VisitsScore;

[TestClass]
public class VisitScoreControllerTest
{
    private Mock<IVisitScoreService> _serviceMock = null!;
    private VisitScoresController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _serviceMock = new Mock<IVisitScoreService>(MockBehavior.Strict);
        _controller = new VisitScoresController(_serviceMock.Object);
    }

    [TestMethod]
    public void GetByVisitor_ValidId_ReturnsMappedList()
    {
        var visitorId = Guid.NewGuid();

        var reg1 = new VisitRegistration();
        var reg2 = new VisitRegistration();

        var s1 = new VisitScore
        {
            Origin = "Attraction",
            OccurredAt = new DateTime(2025, 10, 08, 12, 00, 00, DateTimeKind.Utc),
            Points = 10,
            DayStrategyName = "Attraction",
            VisitRegistration = reg1,
            VisitRegistrationId = reg1.Id
        };
        var s2 = new VisitScore
        {
            Origin = "Canje",
            OccurredAt = new DateTime(2025, 10, 08, 13, 30, 00, DateTimeKind.Utc),
            Points = -5,
            DayStrategyName = null,
            VisitRegistration = reg2,
            VisitRegistrationId = reg2.Id
        };

        _serviceMock
            .Setup(s => s.GetScoresByVisitorId(visitorId))
            .Returns([s1, s2]);

        var result = _controller.GetHistoryById(visitorId.ToString());

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result[0];
        first.Id.Should().Be(s1.Id.ToString());
        first.Origin.Should().Be("Attraction");
        first.OccurredAt.Should().Be(s1.OccurredAt.ToUniversalTime().ToString("O"));
        first.Points.Should().Be(10);
        first.DayStrategyName.Should().Be("Attraction");
        first.VisitRegistrationId.Should().Be(reg1.Id.ToString());

        var second = result[1];
        second.Id.Should().Be(s2.Id.ToString());
        second.Origin.Should().Be("Canje");
        second.OccurredAt.Should().Be(s2.OccurredAt.ToUniversalTime().ToString("O"));
        second.Points.Should().Be(-5);
        second.DayStrategyName.Should().BeNull();
        second.VisitRegistrationId.Should().Be(reg2.Id.ToString());

        _serviceMock.VerifyAll();
    }
}
