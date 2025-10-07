using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
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
    private TicketService _ticketService = null!;

    [TestInitialize]
    public void Setup()
    {
        _ticketRepositoryMock = new Mock<IRepository<Ticket>>();
        _visitorRepositoryMock = new Mock<IRepository<VisitorProfile>>();
        _eventRepositoryMock = new Mock<IRepository<Event>>();
        _attractionRepositoryMock = new Mock<IRepository<Attraction>>();
        _ticketService = new TicketService(_ticketRepositoryMock.Object, _visitorRepositoryMock.Object, _eventRepositoryMock.Object);
    }

    #region Create
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldAddTicketAndReturnEntity()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid().ToString();
        var visitorProfile = new VisitorProfile { Id = visitorId };

        _visitorRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitorProfile);

        var args = new TicketArgs("2025-12-15", "General", eventId, visitorId.ToString());

        var result = _ticketService.Create(args);

        result.Should().NotBeEmpty();
        _ticketRepositoryMock.Verify(r => r.Add(It.IsAny<Ticket>()), Times.Once);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenEventIdIsNull_ShouldAddTicketWithoutEvent()
    {
        var visitorId = Guid.NewGuid();
        var visitorProfile = new VisitorProfile { Id = visitorId };

        _visitorRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitorProfile);

        var args = new TicketArgs("2025-12-15", "General", string.Empty, visitorId.ToString());

        var result = _ticketService.Create(args);

        result.Should().NotBeEmpty();

        _eventRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()), Times.Never);

        _ticketRepositoryMock.Verify(r => r.Add(It.IsAny<Ticket>()), Times.Once);
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
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(ticket);

        _ticketService.Remove(ticketId);

        _ticketRepositoryMock.Verify(r => r.Remove(ticket), Times.Once);
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
            .Setup(r => r.GetAll(null))
            .Returns(tickets);

        var result = _ticketService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(ticket1);
        result.Should().Contain(ticket2);

        _ticketRepositoryMock.Verify(r => r.GetAll(null), Times.Once);
    }
    #endregion
    #region Null
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenNoTicketsExist_ShouldReturnEmptyList()
    {
        _ticketRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Ticket, bool>>>()))
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
        var today = DateTime.Today;

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = today,
            Type = EntranceType.General,
            EventId = Guid.Empty,
            Visitor = new VisitorProfile { Id = visitorId }
        };

        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(ticket);

        var result = _ticketService.IsTicketValidForEntry(visitorId);

        result.Should().BeTrue();
    }
    #endregion
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
    public void ValidateTicket_WhenEventTicketAndCapacityAvailable_ShouldReturnTrue()
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

        var ev = new Event { Id = eventId, Capacity = 2 };

        _ticketRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(ticket);

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        _ticketRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns([ticket]);

        var result = _ticketService.IsTicketValidForEntry(qrId);

        result.Should().BeTrue();
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
            Date = DateTime.Today.AddDays(-1),
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
    #endregion
}
