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
    public void AttractionPoints_ShouldBe110_WhenMixedTypes()
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
            Attractions =
            [
                new Attraction { Type = AttractionType.RollerCoaster },
                new Attraction { Type = AttractionType.Show },
                new Attraction { Type = AttractionType.Simulator },
                new Attraction { Type = (AttractionType)999 }
            ]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _attractionPointsStrategy.CalculatePoints(token);

        result.Should().Be(110);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }
    #endregion

    #region ComboPointsStrategy
    [TestMethod]
    public void ComboPoints_ShouldBeZero_WhenNoAttractions()
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
            Attractions = []
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _comboPointsStrategy.CalculatePoints(token);

        result.Should().Be(0);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [DataTestMethod]
    [DataRow(1, 2)]
    [DataRow(5, 10)]
    [DataRow(10, 20)]
    public void ComboPoints_ShouldCalculateBaseRule_WhenMultipleAttractions(int count, int expected)
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var attractions = Enumerable.Range(0, count)
            .Select(_ => new Attraction { Type = AttractionType.RollerCoaster })
            .ToList();

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Attractions = attractions
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _comboPointsStrategy.CalculatePoints(token);

        result.Should().Be(expected);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [DataTestMethod]
    [DataRow(11, 32)]
    [DataRow(15, 40)]
    [DataRow(25, 60)]
    public void ComboPoints_ShouldCalculateBonusRule_WhenMoreThan10Attractions(int count, int expected)
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var attractions = Enumerable.Range(0, count)
            .Select(_ => new Attraction { Type = AttractionType.RollerCoaster })
            .ToList();

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Attractions = attractions
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _comboPointsStrategy.CalculatePoints(token);

        result.Should().Be(expected);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }
    #endregion

    #region EventPointsStrategy
    [TestMethod]
    public void EventPoints_ShouldBe20_WhenNoEvent()
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile
        {
            Id = visitorId,
            Score = 100
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            Ticket = new Ticket { Event = null },
            Attractions = [new Attraction { Type = AttractionType.RollerCoaster }]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _eventPointsStrategy.CalculatePoints(token);

        result.Should().Be(20);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(10)]
    [DataRow(25)]
    public void EventPoints_ShouldBe20_WhenEventPresent_AndDailyScoreIsZero(int visitorScore)
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile
        {
            Id = visitorId,
            Score = visitorScore
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            DailyScore = 0,
            Ticket = new Ticket { Event = new Event() },
            Attractions = [new Attraction { Type = AttractionType.RollerCoaster }]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _eventPointsStrategy.CalculatePoints(token);

        result.Should().Be(20);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [DataTestMethod]
    [DataRow(0, 0)]
    [DataRow(10, 30)]
    [DataRow(25, 75)]
    public void EventPoints_ShouldBeTriple_WhenEventPresent_AndDailyScoreGreaterThanZero(int visitorScore, int expected)
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile
        {
            Id = visitorId,
            Score = visitorScore
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            DailyScore = 1,
            Ticket = new Ticket { Event = new Event() },
            Attractions = [new Attraction { Type = AttractionType.RollerCoaster }]
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _eventPointsStrategy.CalculatePoints(token);

        result.Should().Be(expected);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(10)]
    [DataRow(25)]
    public void EventPoints_ShouldBeZero_WhenNoEvent(int visitorScore)
    {
        var visitorId = Guid.NewGuid();
        var token = Guid.NewGuid();

        var visitorProfile = new VisitorProfile
        {
            Id = visitorId,
            Score = visitorScore
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            Visitor = visitorProfile,
            IsActive = true,
            DailyScore = 0,
            Ticket = new Ticket { Event = null },
            Attractions = []
        };

        _sessionServiceMock
            .Setup(s => s.GetUserLogged(token))
            .Returns(user);

        _visitRegistrationRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        var result = _eventPointsStrategy.CalculatePoints(token);
        result.Should().Be(0);

        _sessionServiceMock.VerifyAll();
        _visitRegistrationRepositoryMock.VerifyAll();
    }
    #endregion
}
