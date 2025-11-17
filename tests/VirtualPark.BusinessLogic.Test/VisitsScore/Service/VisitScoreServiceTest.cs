using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Service;

namespace VirtualPark.BusinessLogic.Test.VisitsScore.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("VisitScoreService")]
public class VisitScoreServiceTest
{
    private Mock<IVisitScoreRepository> _repoMock = null!;
    private VisitScoreService _service = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repoMock = new Mock<IVisitScoreRepository>(MockBehavior.Strict);
        _service = new VisitScoreService(_repoMock.Object);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetScoresByVisitorId_ShouldReturnScores_FromRepository()
    {
        var visitorId = Guid.NewGuid();
        var expected = new List<VisitScore>
        {
            new VisitScore { Points = 10, Origin = "A", OccurredAt = DateTime.UtcNow.AddMinutes(-2) },
            new VisitScore { Points = 20, Origin = "B", OccurredAt = DateTime.UtcNow }
        };

        _repoMock
            .Setup(r => r.ListByVisitorId(visitorId))
            .Returns(expected);

        var result = _service.GetScoresByVisitorId(visitorId);

        result.Should().BeEquivalentTo(expected);
        _repoMock.Verify(r => r.ListByVisitorId(visitorId), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetScoresByVisitorId_WithEmptyGuid_ShouldThrowArgumentException()
    {
        var empty = Guid.Empty;

        var act = () => _service.GetScoresByVisitorId(empty);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Visitor ID cannot be null.");

        _repoMock.VerifyNoOtherCalls();
    }
}
