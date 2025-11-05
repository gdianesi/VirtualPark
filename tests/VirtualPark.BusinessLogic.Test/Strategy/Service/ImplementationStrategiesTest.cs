using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Strategy.Service;

[TestClass]
public sealed class ImplementationStrategiesTest
{
    private Mock<ISessionService> _sessionServiceMock = null!;
    private Mock<IReadOnlyRepository<VisitRegistration>> _visitRegistrationRepositoryMock = null!;
    private AttractionPointsStrategy _attractionPointsStrategy = null!;
    private ComboPointsStrategy _comboPointsStrategy = null!;
    private EventPointsStrategy _eventPointsStrategy = null!;

    [TestInitialize]
    public void SetUp()
    {
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
        _visitRegistrationRepositoryMock = new Mock<IReadOnlyRepository<VisitRegistration>>(MockBehavior.Strict);
        _attractionPointsStrategy = new AttractionPointsStrategy(_sessionServiceMock.Object, _visitRegistrationRepositoryMock.Object);
        _comboPointsStrategy = new ComboPointsStrategy(_sessionServiceMock.Object, _visitRegistrationRepositoryMock.Object);
        _eventPointsStrategy = new EventPointsStrategy(_sessionServiceMock.Object, _visitRegistrationRepositoryMock.Object);
    }

    #region AttractionPointsStrategy
    [TestMethod]
    public void AttractionPoints_ShouldBeZero_WhenNoAttractions()
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile { Id = visitorId };
        var user = new User { Id = Guid.NewGuid(), VisitorProfile = visitorProfile };
        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Attractions = []
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _attractionPointsStrategy.CalculatePoints(token);

        result.Should().Be(0);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AttractionPoints_ShouldBe50_WhenOneRollerCoaster()
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var user = new User { Id = Guid.NewGuid(), VisitorProfileId = visitorId };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Attractions = [new Attraction { Type = AttractionType.RollerCoaster }]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _attractionPointsStrategy.CalculatePoints(token);

        result.Should().Be(50);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AttractionPoints_ShouldBe30_WhenOneShow()
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Attractions = [new Attraction { Type = AttractionType.Show }]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _attractionPointsStrategy.CalculatePoints(token);

        result.Should().Be(30);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AttractionPoints_ShouldBe10_WhenUnknownType()
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Attractions = [new Attraction { Type = (AttractionType)999 }]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _attractionPointsStrategy.CalculatePoints(token);

        result.Should().Be(10);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AttractionPoints_MixedTypes()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration
        {
            Attractions =
            [
                new() { Type = AttractionType.RollerCoaster },
                new() { Type = AttractionType.Show },
                new() { Type = AttractionType.Simulator },
                new() { Type = (AttractionType)999 }
            ]
        };
        strategy.CalculatePoints(visit.DailyScore).Should().Be(50 + 30 + 20 + 10);
    }
    #endregion

    #region ComboPointsStrategy
    [TestMethod]
    public void ComboPoints_ShouldBeZero_WhenNoAttractions()
    {
        var strategy = new ComboPointsStrategy();
        var visit = new VisitRegistration { Attractions = [] };
        strategy.CalculatePoints(visit).Should().Be(0);
    }

    [DataTestMethod]
    [DataRow(1, 2)]
    [DataRow(5, 10)]
    [DataRow(10, 20)]
    public void ComboPoints_BaseRule(int count, int expected)
    {
        var strategy = new ComboPointsStrategy();
        var visit = new VisitRegistration { Attractions = new List<Attraction>(new Attraction[count]) };
        strategy.CalculatePoints(visit).Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow(11, 32)]
    [DataRow(15, 40)]
    [DataRow(25, 60)]
    public void ComboPoints_BonusRule(int count, int expected)
    {
        var strategy = new ComboPointsStrategy();
        var visit = new VisitRegistration { Attractions = new List<Attraction>(new Attraction[count]) };
        strategy.CalculatePoints(visit).Should().Be(expected);
    }
    #endregion

    #region EventPointsStrategy
    [TestMethod]
    public void EventPoints_ShouldBeZero_WhenNoEvent()
    {
        var strategy = new EventPointsStrategy();
        var visit = new VisitRegistration
        {
            Visitor = new VisitorProfile { Score = 100 },
            Ticket = new Ticket { Event = null },
            Attractions = []
        };
        strategy.CalculatePoints(visit).Should().Be(20);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(10)]
    [DataRow(25)]
    public void EventPoints_ShouldBe20_WhenEventPresent_AndDailyScoreIsZero(int visitorScore)
    {
        var strategy = new EventPointsStrategy();
        var visit = new VisitRegistration
        {
            DailyScore = 0,
            Visitor = new VisitorProfile { Score = visitorScore },
            Ticket = new Ticket { Event = new Event() },
            Attractions = []
        };

        strategy.CalculatePoints(visit).Should().Be(20);
    }

    [DataTestMethod]
    [DataRow(0, 0)]
    [DataRow(10, 30)]
    [DataRow(25, 75)]
    public void EventPoints_ShouldBeTriple_WhenEventPresent_AndDailyScoreGreaterThanZero(int visitorScore, int expected)
    {
        var strategy = new EventPointsStrategy();
        var visit = new VisitRegistration
        {
            DailyScore = 1,
            Visitor = new VisitorProfile { Score = visitorScore },
            Ticket = new Ticket { Event = new Event() },
            Attractions = []
        };

        strategy.CalculatePoints(visit).Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(10)]
    [DataRow(25)]
    public void EventPoints_ShouldBeZero_WhenNoEvent(int visitorScore)
    {
        var strategy = new EventPointsStrategy();
        var visit = new VisitRegistration
        {
            DailyScore = 0,
            Visitor = new VisitorProfile { Score = visitorScore },
            Ticket = new Ticket { Event = null! },
            Attractions = []
        };

        strategy.CalculatePoints(visit).Should().Be(0);
    }
    #endregion
}
