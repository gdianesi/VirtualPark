using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.Tickets.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Tickets.Service;

[TestClass]
public class TicketServiceTest
{
    private Mock<IRepository<Ticket>> _ticketRepositoryMock = null!;
    private Mock<IRepository<VisitorProfile>> _visitorRepositoryMock = null!;
    private Mock<IRepository<Event>> _eventRepositoryMock = null!;
    private Mock<IRepository<Attraction>> _attractionRepositoryMock = null!;
    private Mock<IRepository<Incidence>> _incidenceRepositoryMock = null!;
    private Mock<IClockAppService> _clockMock = null!;
    private Mock<IIncidenceService> _incidenceServiceMock = null!;
    private TicketService _ticketService = null!;

    [TestInitialize]
    public void Setup()
    {
        _ticketRepositoryMock = new Mock<IRepository<Ticket>>(MockBehavior.Strict);
        _visitorRepositoryMock = new Mock<IRepository<VisitorProfile>>(MockBehavior.Strict);
        _eventRepositoryMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
        _attractionRepositoryMock = new Mock<IRepository<Attraction>>(MockBehavior.Strict);
        _clockMock = new Mock<IClockAppService>(MockBehavior.Strict);
        _incidenceRepositoryMock = new Mock<IRepository<Incidence>>(MockBehavior.Strict);
        _incidenceServiceMock = new Mock<IIncidenceService>(MockBehavior.Strict);

        _clockMock.Setup(c => c.Now()).Returns(new DateTime(2025, 12, 15));

        _ticketService = new TicketService(
            _ticketRepositoryMock.Object,
            _visitorRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _clockMock.Object, _incidenceServiceMock.Object);
    }

    #region Create
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldAddTicketAndReturnEntity()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var date = new DateTime(2025, 12, 15);

        var visitorProfile = new VisitorProfile { Id = visitorId };
        var ev = new Event { Id = eventId, Capacity = 10 };

        var args = new TicketArgs(
            date.ToString("yyyy-MM-dd"),
            "General",
            eventId.ToString(),
            visitorId.ToString());

        _visitorRepositoryMock.Setup(r => r.Get(v => v.Id == args.VisitorId))
            .Returns(visitorProfile);

        _eventRepositoryMock.Setup(r => r.Get(e => e.Id == args.EventId.Value))
            .Returns(ev);

        _ticketRepositoryMock.Setup(r => r.Add(It.Is<Ticket>(t =>
            t.Visitor == visitorProfile &&
            t.Event == ev &&
            t.Type == EntranceType.General &&
            t.VisitorProfileId == visitorId &&
            t.EventId == eventId &&
            t.QrId != Guid.Empty &&
            t.Date == date)));

        var result = _ticketService.Create(args);

        result.Should().NotBeEmpty();

        _visitorRepositoryMock.VerifyAll();
        _eventRepositoryMock.VerifyAll();
        _ticketRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenEventIdIsNull_ShouldAddTicketWithoutEvent()
    {
        var visitorId = Guid.NewGuid();
        var date = new DateTime(2025, 12, 15);

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var args = new TicketArgs(
            date.ToString("yyyy-MM-dd"),
            "General",
            string.Empty,
            visitorId.ToString());

        _visitorRepositoryMock.Setup(r => r.Get(v => v.Id == args.VisitorId))
            .Returns(visitorProfile);

        _ticketRepositoryMock.Setup(r => r.Add(It.Is<Ticket>(t =>
            t.Visitor == visitorProfile &&
            t.Event == null &&
            !t.EventId.HasValue &&
            t.Type == EntranceType.General &&
            t.VisitorProfileId == visitorId &&
            t.QrId != Guid.Empty &&
            t.Date == date)));

        var result = _ticketService.Create(args);

        result.Should().NotBeEmpty();

        _visitorRepositoryMock.VerifyAll();
        _ticketRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenAttractionIsUnderMaintenance_ShouldThrowInvalidOperationException()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();

        var date = new DateTime(2025, 12, 15);

        var visitorProfile = new VisitorProfile { Id = visitorId };

        var attraction = new Attraction { Id = attractionId, Name = "RollerCoaster" };

        var ev = new Event
        {
            Id = eventId,
            Attractions = [attraction],
            Capacity = 10
        };

        var args = new TicketArgs(
            date.ToString("yyyy-MM-dd"),
            "Event",
            eventId.ToString(),
            visitorId.ToString());

        _visitorRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitorProfile);

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        _incidenceServiceMock
            .Setup(i => i.HasActiveIncidenceForAttraction(attractionId, date))
            .Returns(true);

        Action act = () => _ticketService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Cannot create ticket: attraction {attraction.Name} is under preventive maintenance at that time.");

        _visitorRepositoryMock.VerifyAll();
        _eventRepositoryMock.VerifyAll();
        _incidenceServiceMock.VerifyAll();
    }

    #endregion

    #region Remove
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenTicketExists_ShouldRemoveFromRepository()
    {
        var ticketId = Guid.NewGuid();
        var ticket = new Ticket { Id = ticketId };

        _ticketRepositoryMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        _ticketRepositoryMock
            .Setup(r => r.Remove(ticket));

        _ticketService.Remove(ticketId);

        _ticketRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenTicketDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var ticketId = Guid.NewGuid();

        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns((Ticket?)null);

        Action act = () => _ticketService.Remove(ticketId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Ticket with id {ticketId} not found.");
    }
    #endregion
    #endregion

    #region Get
    #region Success
    [TestMethod]
    public void Get_WhenTicketExists_ShouldReturnTicket()
    {
        var ticketId = Guid.NewGuid();
        var expected = new Ticket { Id = ticketId };

        _ticketRepositoryMock
            .Setup(r => r.Get(It.Is<Expression<Func<Ticket, bool>>>(expr =>
                expr.Compile().Invoke(new Ticket { Id = ticketId }))))
            .Returns(expected);

        var result = _ticketService.Get(ticketId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(ticketId);

        _ticketRepositoryMock.Verify(
            r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
    }
    #endregion
    #region Null
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Get_WhenTicketDoesNotExist_ShouldReturnNull()
    {
        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns((Ticket?)null);

        var result = _ticketService.Get(Guid.NewGuid());

        result.Should().BeNull();
    }
    #endregion
    #endregion

    #region GetAll
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenTicketsExist_ShouldReturnList()
    {
        var ticket1 = new Ticket { Id = Guid.NewGuid() };
        var ticket2 = new Ticket { Id = Guid.NewGuid() };
        var tickets = new List<Ticket> { ticket1, ticket2 };

        _ticketRepositoryMock
            .Setup(r => r.GetAll())
            .Returns(tickets);

        var result = _ticketService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(ticket1);
        result.Should().Contain(ticket2);

        _ticketRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }
    #endregion
    #region Null
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenNoTicketsExist_ShouldReturnEmptyList()
    {
        _ticketRepositoryMock
            .Setup(r => r.GetAll())
            .Returns([]);

        var result = _ticketService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    #endregion
    #endregion

    #region HasTicketForVisitor
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Exist_WhenTicketWithVisitorExists_ShouldReturnTrue()
    {
        var visitorId = Guid.NewGuid();

        _ticketRepositoryMock
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(true);

        var result = _ticketService.HasTicketForVisitor(visitorId);

        result.Should().BeTrue();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Exist_WhenTicketWithVisitorDoesNotExist_ShouldReturnFalse()
    {
        var visitorId = Guid.NewGuid();

        _ticketRepositoryMock
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(false);

        var result = _ticketService.HasTicketForVisitor(visitorId);

        result.Should().BeFalse();
    }
    #endregion
    #endregion

    #region ValidateTicket
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateTicket_WhenTicketExistsAndIsGeneralAndDateMatches_ShouldReturnTrue()
    {
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = new DateTime(2025, 12, 15),
            Type = EntranceType.General,
            EventId = Guid.Empty,
            Visitor = new VisitorProfile { Id = visitorId }
        };

        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(ticket);

        var result = _ticketService.IsTicketValidForEntry(qrId);

        result.Should().BeTrue();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateTicket_WhenTicketDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var qrId = Guid.NewGuid();
        _ = DateOnly.FromDateTime(DateTime.Today);

        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns((Ticket?)null);

        Action act = () => _ticketService.IsTicketValidForEntry(qrId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"No ticket found with QR: {qrId}");
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateTicket_WhenEventTicketAndCapacityExceeded_ShouldReturnFalse()
    {
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var today = DateTime.Today;
        var eventId = Guid.NewGuid();

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = today,
            Type = EntranceType.Event,
            EventId = eventId,
            Visitor = new VisitorProfile { Id = visitorId }
        };

        var ev = new Event { Id = eventId, Capacity = 1 };

        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(ticket);

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        _ticketRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns([ticket, new Ticket { EventId = eventId }]);

        var result = _ticketService.IsTicketValidForEntry(qrId);

        result.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateTicket_WhenDateIsNotToday_ShouldReturnFalse()
    {
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = new DateTime(2025, 12, 14),
            Type = EntranceType.General,
            EventId = Guid.Empty,
            Visitor = new VisitorProfile { Id = visitorId }
        };

        _ticketRepositoryMock
            .Setup(r => r.Get(t => t.QrId == qrId))
            .Returns(ticket);

        var result = _ticketService.IsTicketValidForEntry(qrId);

        result.Should().BeFalse();
        _ticketRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateTicket_WhenEventTicketAndCapacityAvailable_ShouldReturnTrue()
    {
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var today = new DateTime(2025, 12, 15);

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = today,
            Type = EntranceType.Event,
            EventId = Guid.NewGuid(),
            Visitor = new VisitorProfile { Id = visitorId }
        };

        var ev = new Event { Id = ticket.EventId!.Value, Capacity = 2 };

        _clockMock.Setup(c => c.Now()).Returns(today);
        _ticketRepositoryMock.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _eventRepositoryMock.Setup(r => r.Get(e => e.Id == ticket.EventId)).Returns(ev);
        _ticketRepositoryMock.Setup(r => r.GetAll(t => t.EventId == ticket.EventId)).Returns([ticket]);

        var result = _ticketService.IsTicketValidForEntry(qrId);

        result.Should().BeTrue();

        _clockMock.VerifyAll();
        _ticketRepositoryMock.VerifyAll();
        _eventRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateTicket_WhenEventDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var today = new DateTime(2025, 12, 15);

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = today,
            Type = EntranceType.Event,
            EventId = Guid.NewGuid(),
            Visitor = new VisitorProfile { Id = visitorId }
        };

        _clockMock.Setup(c => c.Now()).Returns(today);
        _ticketRepositoryMock.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _eventRepositoryMock.Setup(r => r.Get(e => e.Id == ticket.EventId)).Returns((Event?)null);

        Action act = () => _ticketService.IsTicketValidForEntry(qrId);

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage($"Event with id {ticket.EventId} not found.");

        _clockMock.VerifyAll();
        _ticketRepositoryMock.VerifyAll();
        _eventRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
