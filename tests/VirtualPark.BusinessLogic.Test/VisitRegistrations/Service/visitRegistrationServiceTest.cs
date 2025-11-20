using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Models;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("VisitRegistrationServiceTest")]
public class VisitRegistrationServiceTest
{
    private Mock<IReadOnlyRepository<VisitorProfile>> _visitorRepoMock = null!;
    private Mock<IRepository<VisitorProfile>> _visitorRepoWriteMock = null!;
    private Mock<IReadOnlyRepository<Attraction>> _attractionRepoMock = null!;
    private Mock<IRepository<VisitRegistration>> _repositoryMock = null!;
    private Mock<IReadOnlyRepository<Ticket>> _ticketRepoMock = null!;
    private Mock<IClockAppService> _clockMock = null!;
    private Mock<IStrategyService> _strategyServiceMock = null!;
    private Mock<IStrategyFactory> _strategyFactoryMock = null!;
    private Mock<IStrategy> _strategyMock = null!;
    private VisitRegistrationService _service = null!;
    private Mock<IReadOnlyRepository<Event>> _eventRepoMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _visitorRepoMock = new Mock<IReadOnlyRepository<VisitorProfile>>(MockBehavior.Strict);
        _visitorRepoWriteMock = new Mock<IRepository<VisitorProfile>>(MockBehavior.Strict);
        _attractionRepoMock = new Mock<IReadOnlyRepository<Attraction>>(MockBehavior.Strict);
        _repositoryMock = new Mock<IRepository<VisitRegistration>>(MockBehavior.Strict);
        _ticketRepoMock = new Mock<IReadOnlyRepository<Ticket>>(MockBehavior.Strict);
        _clockMock = new Mock<IClockAppService>(MockBehavior.Strict);
        _strategyServiceMock = new Mock<IStrategyService>(MockBehavior.Strict);
        _strategyFactoryMock = new Mock<IStrategyFactory>(MockBehavior.Strict);
        _strategyMock = new Mock<IStrategy>(MockBehavior.Strict);
        _eventRepoMock = new Mock<IReadOnlyRepository<Event>>(MockBehavior.Strict);

        var mockClockForValidation = new Mock<IClockAppService>();
        mockClockForValidation.Setup(x => x.Now()).Returns(new DateTime(2025, 10, 7, 12, 0, 0));
        ValidationServices.ClockService = mockClockForValidation.Object;

        _service = new VisitRegistrationService(
            _repositoryMock.Object,
            _visitorRepoMock.Object,
            _attractionRepoMock.Object,
            _ticketRepoMock.Object,
            _clockMock.Object,
            _visitorRepoWriteMock.Object,
            _strategyServiceMock.Object,
            _strategyFactoryMock.Object,
            _eventRepoMock.Object);
    }

    #region Create
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldCreateVisitRegistration_WhenVisitorAndAttractionsExist()
    {
        var fixedNow = new DateTime(2025, 10, 08, 14, 30, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(fixedNow);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var a1 = new Attraction { Name = "Roller" };
        var a2 = new Attraction { Name = "Wheel" };
        var a1Id = a1.Id;
        var a2Id = a2.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var args = new VisitRegistrationArgs(
            [a1Id.ToString(), a2Id.ToString()],
            visitorId.ToString(),
            ticketId.ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1Id))
            .Returns(a1);
        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a2Id))
            .Returns(a2);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        _repositoryMock
            .Setup(r => r.Add(It.Is<VisitRegistration>(vr =>
                vr.VisitorId == visitorId &&
                vr.Visitor == visitor &&
                vr.TicketId == ticketId &&
                vr.Ticket == ticket &&
                vr.Attractions.Count == 2 &&
                vr.Attractions[0].Id == a1Id &&
                vr.Attractions[1].Id == a2Id &&
                vr.Date == fixedNow)));

        var result = _service.Create(args);

        result.Should().NotBeNull();
        result.VisitorId.Should().Be(visitorId);
        result.Visitor.Should().BeSameAs(visitor);
        result.TicketId.Should().Be(ticketId);
        result.Ticket.Should().BeSameAs(ticket);
        result.Attractions.Should().HaveCount(2);
        result.Attractions[0].Id.Should().Be(a1Id);
        result.Attractions[1].Id.Should().Be(a2Id);
        result.Date.Should().Be(fixedNow);

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _clockMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenVisitorDoesNotExist()
    {
        var visitorId = Guid.NewGuid();
        var args = new VisitRegistrationArgs([], visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns((VisitorProfile?)null);

        Action act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenAnyAttractionDoesNotExist()
    {
        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var missingId = Guid.NewGuid();

        var args = new VisitRegistrationArgs(
            [missingId.ToString()],
            visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == missingId))
            .Returns((Attraction?)null);

        var act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenTicketDoesNotExist()
    {
        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var ticketId = Guid.NewGuid();
        var args = new VisitRegistrationArgs([], visitorId.ToString(), ticketId.ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns((Ticket?)null);

        Action act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Ticket don't exist");

        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Get
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldReturnVisitRegistration_WhenExists()
    {
        var visit = new VisitRegistration();
        var id = visit.Id;

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;
        visit.VisitorId = visitorId;

        var ticket = new Ticket();
        var ticketId = ticket.Id;
        visit.TicketId = ticketId;

        var a1 = new Attraction { Name = "Placeholder" };
        visit.Attractions = [a1];

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns(a1);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        var result = _service.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Visitor.Should().BeSameAs(visitor);
        result.VisitorId.Should().Be(visitor.Id);
        result.Ticket.Should().BeSameAs(ticket);
        result.TicketId.Should().Be(ticket.Id);
        result.Attractions[0].Id.Should().Be(a1.Id);

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldThrow_WhenVisitRegistrationDoesNotExist()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldThrow_WhenAnyAttractionDoesNotExist()
    {
        var visit = new VisitRegistration();
        var id = visit.Id;

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;
        visit.VisitorId = visitorId;

        var ticket = new Ticket();
        var ticketId = ticket.Id;
        visit.TicketId = ticketId;

        var a1 = new Attraction { Name = "Placeholder" };
        visit.Attractions = [a1];

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns((Attraction?)null);

        var act = () => _service.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Remove
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldDeleteVisitRegistration_WhenExists()
    {
        var vr = new VisitRegistration();
        var id = vr.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(vr);

        _repositoryMock
            .Setup(r => r.Remove(vr));

        _service.Remove(id);

        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldThrow_WhenVisitRegistrationDoesNotExist()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Update
    [TestMethod]
    [TestCategory("Validation")]
    public void Update_ShouldApplyChanges_AndPersist_WhenVisitExists()
    {
        var visit = new VisitRegistration();
        var visitId = visit.Id;

        var oldVisitor = new VisitorProfile();
        var visitorId = oldVisitor.Id;
        visit.VisitorId = visitorId;

        var oldTicket = new Ticket();
        var ticketId = oldTicket.Id;
        visit.TicketId = ticketId;

        visit.Attractions = [];

        var newVisitorId = Guid.NewGuid();
        var newTicket = new Ticket();
        var newTicketId = newTicket.Id;

        var args = new VisitRegistrationArgs(
            [],
            newVisitorId.ToString(),
            newTicketId.ToString());

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(oldVisitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(oldTicket);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == newTicketId))
            .Returns(newTicket);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(oldVisitor);

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(vr =>
                vr.Id == visitId &&
                vr.Ticket == newTicket &&
                vr.TicketId == newTicketId &&
                vr.Visitor == oldVisitor &&
                vr.VisitorId == newVisitorId)));

        _service.Update(args, visitId);

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion

    #region GetAll
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldReturnVisitRegistrations_WithAllData()
    {
        var v1 = new VisitRegistration();
        var v2 = new VisitRegistration();

        var vp1 = new VisitorProfile();
        var vp1Id = vp1.Id;
        v1.VisitorId = vp1Id;
        var vp2 = new VisitorProfile();
        var vp2Id = vp2.Id;
        v2.VisitorId = vp2Id;

        var t1 = new Ticket();
        var t1Id = t1.Id;
        v1.TicketId = t1Id;
        var t2 = new Ticket();
        var t2Id = t2.Id;
        v2.TicketId = t2Id;

        var a1 = new Attraction { Name = "A1" };
        var a1Id = a1.Id;

        v1.Attractions = [a1];

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns([v1, v2]);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == t1Id))
            .Returns(t1);
        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == t2Id))
            .Returns(t2);

        _visitorRepoMock
            .Setup(r => r.Get(vp => vp.Id == vp1Id))
            .Returns(vp1);
        _visitorRepoMock
            .Setup(r => r.Get(vp => vp.Id == vp2Id))
            .Returns(vp2);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns(a1);

        var result = _service.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var r1 = result[0];
        r1.Visitor.Should().BeSameAs(vp1);
        r1.Ticket.Should().BeSameAs(t1);
        r1.Attractions.Should().HaveCount(1);
        r1.Attractions[0].Id.Should().Be(a1Id);

        var r2 = result[1];
        r2.Visitor.Should().BeSameAs(vp2);
        r2.Ticket.Should().BeSameAs(t2);

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldThrow_WhenRepositoryReturnsNull()
    {
        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns((List<VisitRegistration>)null!);

        var act = _service.GetAll;

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Dont have any visit registrations");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion
    #endregion

    #region RecordVisitScore
    [TestMethod]
    [TestCategory("Validation")]
    public void RecordVisitScore_ShouldThrow_WhenNonRedemptionHasPoints()
    {
        var now = new DateTime(2025, 10, 08, 13, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = new DateOnly(2025, 10, 08);

        var visitor = new VisitorProfile { Score = 0 };
        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 0,
            VisitorId = visitor.Id,
            Visitor = visitor,
            Ticket = new Ticket(),
            Attractions = [],
            ScoreEvents = []
        };
        var visitId = visit.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns(visit);

        _strategyServiceMock
            .Setup(s => s.Get(today))
            .Returns(new ActiveStrategyArgs("Attraction", today.ToString("yyyy-MM-dd")));

        var args = new RecordVisitScoreArgs(visitId.ToString(), "Atracción", "10");

        Action act = () => _service.RecordVisitScore(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Points solo se permite para 'Canje'; para otros orígenes deje null.");

        _repositoryMock.VerifyAll();
        _clockMock.VerifyAll();
        _strategyServiceMock.VerifyAll();
        _strategyFactoryMock.VerifyNoOtherCalls();
        _visitorRepoWriteMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void RecordVisitScore_ShouldThrow_WhenVisitNotFoundById()
    {
        var visitId = Guid.NewGuid();

        _clockMock.Setup(c => c.Now()).Returns(new DateTime(2025, 10, 08, 12, 0, 0, DateTimeKind.Utc));

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.RecordVisitScore(
            new RecordVisitScoreArgs(visitId.ToString(), "Atracción", null));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"VisitRegistration {visitId} not found.");

        _repositoryMock.VerifyAll();
        _clockMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void RecordVisitScore_FirstEvent_ShouldFreezeStrategyInEvent_AddEvent_AndApplyDelta()
    {
        var now = new DateTime(2025, 10, 08, 12, 00, 00, DateTimeKind.Utc);
        var today = new DateOnly(2025, 10, 08);

        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile { Score = 0 };
        var ticket = new Ticket();
        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 0,
            VisitorId = visitor.Id,
            Visitor = visitor,
            Ticket = ticket,
            Attractions = [],
            ScoreEvents = []
        };
        var visitId = visit.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns(visit);

        _strategyServiceMock
            .Setup(s => s.Get(today))
            .Returns(new ActiveStrategyArgs("Attraction", today.ToString("yyyy-MM-dd")));

        _strategyFactoryMock
            .Setup(f => f.Create("Attraction"))
            .Returns(_strategyMock.Object);

        _strategyMock
            .Setup(s => s.CalculatePoints(visitor.Id))
            .Returns(10);

        _visitorRepoWriteMock
            .Setup(w => w.Update(It.Is<VisitorProfile>(vp => vp.Score == 10)))
            .Verifiable();

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(v =>
                v.DailyScore == 10 &&
                v.ScoreEvents.Count == 1 &&
                v.ScoreEvents[0].Origin == "Atracción" &&
                v.ScoreEvents[0].Points == 10 &&
                v.ScoreEvents[0].DayStrategyName == "Attraction" &&
                v.ScoreEvents[0].VisitRegistrationId == visitId)))
            .Verifiable();

        _service.RecordVisitScore(new RecordVisitScoreArgs(visitId.ToString(), "Atracción", null));

        visit.DailyScore.Should().Be(10);
        visitor.Score.Should().Be(10);
        visit.ScoreEvents.Should().HaveCount(1);
        var ev = visit.ScoreEvents[0];
        ev.DayStrategyName.Should().Be("Attraction");
        ev.Points.Should().Be(10);

        _repositoryMock.VerifyAll();
        _visitorRepoWriteMock.VerifyAll();
        _strategyServiceMock.VerifyAll();
        _strategyFactoryMock.VerifyAll();
        _strategyMock.VerifyAll();
        _clockMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void RecordVisitScore_ShouldThrow_WhenRedemptionWithoutPoints()
    {
        var now = new DateTime(2025, 10, 08, 11, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile { Score = 100 };

        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 100,
            VisitorId = visitor.Id,
            Visitor = visitor,
            Ticket = new Ticket(),
            Attractions = [],
            ScoreEvents =
            [
                new VisitScore
                {
                    Origin = "Seed",
                    OccurredAt = now.AddMinutes(-1),
                    Points = 0,
                    DayStrategyName = "Attraction",
                    VisitRegistrationId = Guid.Empty
                }

            ]
        };
        visit.ScoreEvents[0].VisitRegistrationId = visit.Id;
        visit.ScoreEvents[0].VisitRegistration = visit;

        var visitId = visit.Id;

        _repositoryMock.Setup(r => r.Get(v => v.Id == visitId)).Returns(visit);

        var args = new RecordVisitScoreArgs(visitId.ToString(), "Canje", null);

        Action act = () => _service.RecordVisitScore(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Points es requerido para origen 'Canje'.");

        _repositoryMock.VerifyAll();
        _clockMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void RecordVisitScore_RedemptionWithZeroDelta_ShouldRecordEvent_AndNotUpdateVisitor()
    {
        var now = new DateTime(2025, 10, 08, 19, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile { Score = 75 };
        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 75,
            VisitorId = visitor.Id,
            Visitor = visitor,
            Ticket = new Ticket(),
            Attractions = [],
            ScoreEvents =
            [
                new VisitScore { Origin = "Seed", OccurredAt = now.AddMinutes(-2), Points = 0, DayStrategyName = "Attraction" }
            ]
        };
        visit.ScoreEvents[0].VisitRegistration = visit;
        visit.ScoreEvents[0].VisitRegistrationId = visit.Id;

        var visitId = visit.Id;

        _repositoryMock.Setup(r => r.Get(v => v.Id == visitId)).Returns(visit);

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(v =>
                v.DailyScore == 75 &&
                v.ScoreEvents.Count == 2 &&
                v.ScoreEvents[1].Origin == "Canje" &&
                v.ScoreEvents[1].Points == 0 &&
                v.ScoreEvents[1].VisitRegistrationId == visitId)))
            .Verifiable();

        _visitorRepoWriteMock.VerifyNoOtherCalls();

        _service.RecordVisitScore(new RecordVisitScoreArgs(visitId.ToString(), "Canje", "0"));

        visit.DailyScore.Should().Be(75);
        visitor.Score.Should().Be(75);
        visit.ScoreEvents.Should().HaveCount(2);
        visit.ScoreEvents[1].Points.Should().Be(0);

        _repositoryMock.VerifyAll();
        _clockMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void RecordVisitScore_FirstEvent_ShouldStoreTrimmedOrigin()
    {
        var now = new DateTime(2025, 10, 08, 10, 00, 00, DateTimeKind.Utc);
        var today = new DateOnly(2025, 10, 08);

        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile { Score = 0 };
        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 0,
            VisitorId = visitor.Id,
            Visitor = visitor,
            Ticket = new Ticket(),
            Attractions = [],
            ScoreEvents = []
        };
        var visitId = visit.Id;

        _repositoryMock.Setup(r => r.Get(v => v.Id == visitId)).Returns(visit);

        _strategyServiceMock
            .Setup(s => s.Get(today))
            .Returns(new ActiveStrategyArgs("Attraction", today.ToString("yyyy-MM-dd")));

        _strategyFactoryMock
            .Setup(f => f.Create("Attraction"))
            .Returns(_strategyMock.Object);

        _strategyMock
            .Setup(s => s.CalculatePoints(visitor.Id))
            .Returns(10);

        _visitorRepoWriteMock
            .Setup(w => w.Update(It.Is<VisitorProfile>(vp => vp.Score == 10)))
            .Verifiable();

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(v =>
                v.DailyScore == 10 &&
                v.ScoreEvents.Count == 1 &&
                v.ScoreEvents[0].Origin == "Atracción" &&
                v.ScoreEvents[0].Points == 10 &&
                v.ScoreEvents[0].DayStrategyName == "Attraction" &&
                v.ScoreEvents[0].VisitRegistrationId == visitId)))
            .Verifiable();

        _service.RecordVisitScore(new RecordVisitScoreArgs(visitId.ToString(), "  Atracción  ", null));

        visit.DailyScore.Should().Be(10);
        visitor.Score.Should().Be(10);
        visit.ScoreEvents.Should().HaveCount(1);
        visit.ScoreEvents[0].Origin.Should().Be("Atracción");

        _repositoryMock.VerifyAll();
        _visitorRepoWriteMock.VerifyAll();
        _strategyServiceMock.VerifyAll();
        _strategyFactoryMock.VerifyAll();
        _strategyMock.VerifyAll();
        _clockMock.VerifyAll();
    }
    #endregion

    #region UpToAttraction
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void UpToAttraction_ShouldThrow_WhenVisitorIsAlreadyOnAttraction()
    {
        var now = new DateTime(2025, 10, 08, 13, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var currentAttraction = new Attraction { Name = "Roller Coaster" };

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Attractions = [],
            CurrentAttraction = currentAttraction,
            CurrentAttractionId = currentAttraction.Id,
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        var newAttractionId = Guid.NewGuid();

        Action act = () => _service.UpToAttraction(visitorId, newAttractionId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor is already on an attraction.");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void UpToAttraction_ShouldSetCurrentAttraction_AndPersist_WhenVisitForTodayExists()
    {
        var now = new DateTime(2025, 10, 08, 12, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        var target = new Attraction { Name = "Roller Coaster" };
        var targetId = target.Id;

        _attractionRepoMock
            .Setup(r => r.Get(a => a.Id == targetId))
            .Returns(target);

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(vr =>
                vr.VisitorId == visitorId &&
                vr.CurrentAttraction == target &&
                vr.CurrentAttractionId == targetId)));

        _service.UpToAttraction(visitorId, targetId);

        visitToday.CurrentAttraction.Should().BeSameAs(target);
        visitToday.CurrentAttractionId.Should().Be(targetId);

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }
    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void UpToAttraction_ShouldThrow_WhenRepositoryReturnsNull()
    {
        var visitorId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();
        var now = new DateTime(2025, 10, 08, 9, 0, 0, DateTimeKind.Utc);

        _clockMock
            .Setup(c => c.Now())
            .Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.UpToAttraction(visitorId, attractionId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void UpToAttraction_ShouldThrow_WhenVisitForTodayDoesNotExistForVisitor()
    {
        var now = new DateTime(2025, 10, 08, 10, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitorId = Guid.NewGuid();
        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.UpToAttraction(visitorId, Guid.NewGuid());

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void UpToAttraction_ShouldThrow_WhenAttractionDoesNotExist()
    {
        var now = new DateTime(2025, 10, 08, 11, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        var missingAttractionId = Guid.NewGuid();

        _attractionRepoMock
            .Setup(r => r.Get(a => a.Id == missingAttractionId))
            .Returns((Attraction?)null);

        Action act = () => _service.UpToAttraction(visitorId, missingAttractionId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }
    #endregion
    #endregion

    #region DownToAttraction
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void RecordVisitScore_ShouldThrow_WhenNoActiveStrategyForToday()
    {
        var now = new DateTime(2025, 10, 08, 13, 30, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = new DateOnly(2025, 10, 08);

        var visitor = new VisitorProfile { Score = 0 };
        var ticket = new Ticket();

        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 0,
            VisitorId = visitor.Id,
            Visitor = visitor,
            TicketId = ticket.Id,
            Ticket = ticket,
            Attractions = [],
            ScoreEvents = []
        };
        var visitId = visit.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns(visit);

        _strategyServiceMock
            .Setup(s => s.Get(today))
            .Returns((ActiveStrategyArgs?)null);

        var args = new RecordVisitScoreArgs(visitId.ToString(), "Atracción", null);

        Action act = () => _service.RecordVisitScore(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"No active strategy for {today}.");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _strategyServiceMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _strategyFactoryMock.VerifyNoOtherCalls();
        _visitorRepoWriteMock.VerifyNoOtherCalls();
        _strategyMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void DownToAttraction_ShouldDeactivateVisit_WhenCurrentAttractionIsSet()
    {
        var now = new DateTime(2025, 10, 08, 18, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile { Score = 0 };
        var visitorId = visitor.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var attraction = new Attraction { Name = "Montaña Rusa" };

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = now,
            TicketId = ticketId,
            Ticket = ticket,
            Attractions = [],
            CurrentAttraction = attraction,
            CurrentAttractionId = attraction.Id,
            DailyScore = 0,
            ScoreEvents = []
        };
        var visitId = visitToday.Id;

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        _repositoryMock
            .Setup(r => r.Update(visitToday));

        _service.DownToAttraction(visitorId);

        visitToday.DailyScore.Should().Be(0);
        visitor.Score.Should().Be(0);
        visitToday.ScoreEvents.Should().BeEmpty();

        visitToday.CurrentAttraction.Should().BeNull();
        visitToday.CurrentAttractionId.Should().BeNull();
        visitToday.IsActive.Should().BeFalse();

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _strategyServiceMock.VerifyNoOtherCalls();
        _strategyFactoryMock.VerifyNoOtherCalls();
        _strategyMock.VerifyNoOtherCalls();
        _visitorRepoWriteMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void DownToAttraction_ShouldDeactivateVisit_WhenCurrentAttractionNameIsNullOrWhitespace()
    {
        var now = new DateTime(2025, 10, 08, 19, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile { Score = 0 };
        var visitorId = visitor.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var attraction = new Attraction { Name = "   " };

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = now,
            TicketId = ticketId,
            Ticket = ticket,
            Attractions = [],
            CurrentAttraction = attraction,
            CurrentAttractionId = attraction.Id,
            DailyScore = 0,
            ScoreEvents = []
        };
        var visitId = visitToday.Id;

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        _repositoryMock
            .Setup(r => r.Update(visitToday));

        _service.DownToAttraction(visitorId);

        visitToday.DailyScore.Should().Be(0);
        visitor.Score.Should().Be(0);
        visitToday.ScoreEvents.Should().BeEmpty();

        visitToday.CurrentAttraction.Should().BeNull();
        visitToday.CurrentAttractionId.Should().BeNull();
        visitToday.IsActive.Should().BeFalse();

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _strategyServiceMock.VerifyNoOtherCalls();
        _strategyFactoryMock.VerifyNoOtherCalls();
        _strategyMock.VerifyNoOtherCalls();
        _visitorRepoWriteMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void DownToAttraction_WhenAlreadyNull_ShouldNotCallUpdate()
    {
        var now = new DateTime(2025, 10, 08, 14, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Attractions = [],
            CurrentAttraction = null,
            CurrentAttractionId = null,
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        _service.DownToAttraction(visitorId);

        _repositoryMock.Verify(r => r.Update(It.IsAny<VisitRegistration>()), Times.Never);

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void DownToAttraction_ShouldThrow_WhenRepositoryReturnsNull()
    {
        var visitorId = Guid.NewGuid();
        var now = new DateTime(2025, 10, 08, 15, 0, 0, DateTimeKind.Utc);

        _clockMock
            .Setup(c => c.Now())
            .Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.DownToAttraction(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void DownToAttraction_ShouldThrow_WhenVisitForTodayDoesNotExistForVisitor()
    {
        var now = new DateTime(2025, 10, 08, 16, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitorId = Guid.NewGuid();

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.DownToAttraction(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region GetAttractionsForTicket
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAttractionsForTicket_ShouldReturnAllAttractions_WhenTicketIsGeneral()
    {
        var now = new DateTime(2025, 10, 08, 10, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var generalTicket = new Ticket
        {
            Type = EntranceType.General
        };
        var ticketId = generalTicket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Ticket = null!,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(generalTicket);

        var a1 = new Attraction { Name = "Roller" };
        var a2 = new Attraction { Name = "Wheel" };
        var allAttractions = new List<Attraction> { a1, a2 };

        _attractionRepoMock
            .Setup(r => r.GetAll())
            .Returns(allAttractions);

        var result = _service.GetAttractionsForTicket(visitorId);

        result.Should().HaveCount(2);
        result[0].Should().BeSameAs(a1);
        result[1].Should().BeSameAs(a2);

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAttractionsForTicket_ShouldReturnEventAttractions_WhenTicketIsEvent()
    {
        var now = new DateTime(2025, 10, 08, 12, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var eventId = Guid.NewGuid();
        var eventTicket = new Ticket
        {
            Type = EntranceType.Event,
            EventId = eventId
        };
        var ticketId = eventTicket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Ticket = null!,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(It.Is<Expression<Func<VisitorProfile, bool>>>(expr => expr.Compile().Invoke(visitor))))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(eventTicket);

        var ev = new Event
        {
            Id = eventId,
            Attractions =
            [
                new Attraction { Id = Guid.NewGuid() },
                new Attraction { Id = Guid.NewGuid() }
            ]
        };

        _eventRepoMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(ev);

        var a1 = new Attraction { Id = ev.Attractions[0].Id, Name = "Casa del Terror" };
        var a2 = new Attraction { Id = ev.Attractions[1].Id, Name = "Labyrinth" };

        var attrIndex = 0;
        _attractionRepoMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Expression<Func<Attraction, bool>> expr) =>
            {
                var attractions = new[] { a1, a2 };
                var chosen = attractions.First(x => expr.Compile().Invoke(x));
                return chosen;
            });

        var result = _service.GetAttractionsForTicket(visitorId);

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(a1.Id);
        result[1].Id.Should().Be(a2.Id);

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _eventRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenRepositoryReturnsNullVisitRegistrations()
    {
        var visitorId = Guid.NewGuid();
        var now = new DateTime(2025, 10, 08, 10, 00, 00, DateTimeKind.Utc);

        _clockMock
            .Setup(c => c.Now())
            .Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenVisitForTodayDoesNotExistForVisitor()
    {
        var now = new DateTime(2025, 10, 08, 10, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenTicketEventIsNull()
    {
        var now = new DateTime(2025, 10, 08, 20, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var eventTicket = new Ticket
        {
            Type = EntranceType.Event,
            Event = null,
            EventId = null
        };
        var ticketId = eventTicket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Ticket = null!,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(eventTicket);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Ticket event don't have attractions");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenTicketTypeIsUnsupported()
    {
        var now = new DateTime(2025, 10, 08, 22, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var weirdType = (EntranceType)999;

        var weirdTicket = new Ticket
        {
            Type = weirdType
        };
        var ticketId = weirdTicket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Ticket = null!,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(weirdTicket);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Unsupported ticket type: {weirdType}");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenVisitHasNullTicket()
    {
        var now = new DateTime(2025, 10, 08, 23, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;
        var ticketId = Guid.NewGuid();

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            Ticket = null!,
            TicketId = ticketId,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns((Ticket?)null);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Ticket don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenAttractionsRepositoryReturnsNull_ForGeneralTicket()
    {
        var now = new DateTime(2025, 10, 08, 9, 00, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var generalTicket = new Ticket
        {
            Type = EntranceType.General
        };
        var ticketId = generalTicket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Ticket = null!,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(v =>
                v.VisitorId == visitorId &&
                v.Date >= start &&
                v.Date <= end))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(generalTicket);

        _attractionRepoMock
            .Setup(r => r.GetAll())
            .Returns((List<Attraction>)null!);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Dont have any attractions");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAttractionsForTicket_ShouldThrow_WhenEventDoesNotExist()
    {
        var now = new DateTime(2025, 10, 08, 20, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var eventTicket = new Ticket
        {
            Type = EntranceType.Event,
            EventId = Guid.NewGuid()
        };
        var ticketId = eventTicket.Id;

        var visitToday = new VisitRegistration
        {
            VisitorId = visitorId,
            Date = now,
            TicketId = ticketId,
            Ticket = null!,
            Attractions = [],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visitToday);

        _visitorRepoMock
            .Setup(r => r.Get(It.Is<Expression<Func<VisitorProfile, bool>>>(expr => expr.Compile().Invoke(visitor))))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(eventTicket);

        _eventRepoMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns((Event?)null);

        Action act = () => _service.GetAttractionsForTicket(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Event don't exist");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _eventRepoMock.VerifyAll();
    }
    #endregion

    #region GetVisitorsInAttraction
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetVisitorsInAttraction_ShouldReturnActiveVisitorsForToday_InAttraction()
    {
        var now = new DateTime(2025, 10, 09, 15, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var attractionId = Guid.NewGuid();

        var vp1 = new VisitorProfile { Id = Guid.NewGuid() };
        var vp2 = new VisitorProfile { Id = Guid.NewGuid() };
        var vp3 = new VisitorProfile { Id = Guid.NewGuid() };

        var ticket = new Ticket
        {
            Type = EntranceType.General
        };

        var visitTodayInAttraction = new VisitRegistration
        {
            VisitorId = vp1.Id,
            Visitor = vp1,
            Date = now,
            IsActive = true,
            CurrentAttractionId = attractionId,
            Ticket = ticket,
            TicketId = ticket.Id
        };

        var visitTodayDifferentAttraction = new VisitRegistration
        {
            VisitorId = vp2.Id,
            Visitor = vp2,
            Date = now,
            IsActive = true,
            CurrentAttractionId = Guid.NewGuid()
        };

        var visitOtherDaySameAttraction = new VisitRegistration
        {
            VisitorId = vp3.Id,
            Visitor = vp3,
            Date = now.AddDays(-1),
            IsActive = true,
            CurrentAttractionId = attractionId
        };

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(
            [
                visitTodayInAttraction,
                visitTodayDifferentAttraction,
                visitOtherDaySameAttraction
            ]);

        var result = _service.GetVisitorsInAttraction(attractionId);

        result.Should().HaveCount(1);
        var item = result[0];
        item.VisitRegistrationId.Should().Be(visitTodayInAttraction.Id);
        item.Visitor.Should().BeSameAs(vp1);
        item.Visitor.Id.Should().Be(vp1.Id);
        item.TicketType.Should().Be(EntranceType.General);

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetVisitorsInAttraction_ShouldReturnEmpty_WhenNoActiveVisitsForTodayInAttraction()
    {
        var now = new DateTime(2025, 10, 09, 16, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var attractionId = Guid.NewGuid();

        var vp1 = new VisitorProfile { Id = Guid.NewGuid() };
        var vp2 = new VisitorProfile { Id = Guid.NewGuid() };

        var inactiveVisitSameAttraction = new VisitRegistration
        {
            VisitorId = vp1.Id,
            Visitor = vp1,
            Date = now,
            IsActive = false,
            CurrentAttractionId = attractionId
        };

        var activeVisitOtherAttraction = new VisitRegistration
        {
            VisitorId = vp2.Id,
            Visitor = vp2,
            Date = now,
            IsActive = true,
            CurrentAttractionId = Guid.NewGuid()
        };

        var activeVisitSameAttractionOtherDay = new VisitRegistration
        {
            VisitorId = vp1.Id,
            Visitor = vp1,
            Date = now.AddDays(-1),
            IsActive = true,
            CurrentAttractionId = attractionId
        };

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(
            [
                inactiveVisitSameAttraction,
                activeVisitOtherAttraction,
                activeVisitSameAttractionOtherDay
            ]);

        var result = _service.GetVisitorsInAttraction(attractionId);

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetVisitorsInAttraction_ShouldThrow_WhenRepositoryReturnsNull()
    {
        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns((List<VisitRegistration>)null!);

        var attractionId = Guid.NewGuid();

        Action act = () => _service.GetVisitorsInAttraction(attractionId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Dont have any visit registrations");

        _repositoryMock.VerifyAll();
        _clockMock.VerifyNoOtherCalls();
        _visitorRepoMock.VerifyNoOtherCalls();
    }
    #endregion

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetTodayVisit_ShouldReturnVisit_WithLoadedData()
    {
        var now = new DateTime(2025, 10, 10, 11, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var today = DateOnly.FromDateTime(now);
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = today.ToDateTime(TimeOnly.MaxValue);

        var visitor = new VisitorProfile { Id = Guid.NewGuid(), Score = 0 };
        var ticket = new Ticket { Type = EntranceType.General };
        var attraction = new Attraction { Name = "Coaster" };

        var visit = new VisitRegistration
        {
            VisitorId = visitor.Id,
            TicketId = ticket.Id,
            Date = now,
            Attractions =
            [
                new Attraction { Id = attraction.Id }
            ],
            ScoreEvents = []
        };

        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(It.Is<Expression<Func<VisitorProfile, bool>>>(expr => expr.Compile().Invoke(visitor))))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(It.Is<Expression<Func<Ticket, bool>>>(expr => expr.Compile().Invoke(ticket))))
            .Returns(ticket);

        _attractionRepoMock
            .Setup(r => r.Get(It.Is<Expression<Func<Attraction, bool>>>(expr => expr.Compile().Invoke(attraction))))
            .Returns(attraction);

        var result = _service.GetTodayVisit(visitor.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(visit.Id);
        result.Visitor.Should().BeSameAs(visitor);
        result.Ticket.Should().BeSameAs(ticket);
        result.Attractions.Should().HaveCount(1);
        result.Attractions[0].Id.Should().Be(attraction.Id);

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void RecordVisitScore_ShouldNotUpdateVisitor_WhenDeltaIsZero()
    {
        var now = new DateTime(2025, 10, 08, 18, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        var visitor = new VisitorProfile { Id = Guid.NewGuid(), Score = 100, PointsAvailable = 100 };
        var visit = new VisitRegistration
        {
            Date = now,
            DailyScore = 100,
            VisitorId = visitor.Id,
            Visitor = visitor,
            Ticket = new Ticket(),
            Attractions = [],
            ScoreEvents =
            [
                new VisitScore { Origin = "Seed", OccurredAt = now.AddMinutes(-5), Points = 0, DayStrategyName = "Attraction" }
            ]
        };
        var visitId = visit.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns(visit);

        _strategyFactoryMock
            .Setup(f => f.Create("Attraction"))
            .Returns(_strategyMock.Object);

        _strategyMock
            .Setup(s => s.CalculatePoints(visitor.Id))
            .Returns(100);

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(vr =>
                vr.DailyScore == 100 &&
                vr.ScoreEvents.Count == 2 &&
                vr.ScoreEvents[1].Points == 0 &&
                vr.ScoreEvents[1].Origin == "Atracción")))
            .Verifiable();

        _service.RecordVisitScore(new RecordVisitScoreArgs(visitId.ToString(), "Atracción", null));

        visitor.Score.Should().Be(100);
        visitor.PointsAvailable.Should().Be(100);

        _repositoryMock.VerifyAll();
        _strategyFactoryMock.VerifyAll();
        _strategyMock.VerifyAll();
        _visitorRepoWriteMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetTodayVisit_ShouldThrow_WhenVisitNotFound()
    {
        var visitorId = Guid.NewGuid();
        var now = new DateTime(2025, 10, 10, 9, 0, 0, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(now);

        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.GetTodayVisit(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("VisitRegistration for today don't exist");

        _clockMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }
}
