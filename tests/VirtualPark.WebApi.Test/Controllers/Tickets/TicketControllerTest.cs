using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.Tickets.Service;
using VirtualPark.WebApi.Controllers.Tickets;
using VirtualPark.WebApi.Controllers.Tickets.ModelsIn;
using VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Tickets;

[TestClass]
public sealed class TicketControllerTest
{
    private TicketController _controller = null!;
    private Mock<ITicketService> _ticketServiceMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _ticketServiceMock = new Mock<ITicketService>(MockBehavior.Strict);
        _controller = new TicketController(_ticketServiceMock.Object);
    }

    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void GetTicketById_WhenGuidIsInvalid_ShouldThrowFormatException()
    {
        const string invalidId = "NOT_A_GUID";

        Action act = () => _controller.GetTicketById(invalidId);

        act.Should()
            .Throw<FormatException>()
            .WithMessage($"The value '{invalidId}' is not a valid GUID.");
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetTicketById_ValidInput_ShouldReturnGetTicketResponse()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 12, 15),
            Type = EntranceType.Event,
            EventId = eventId,
            VisitorProfileId = visitorId,
            QrId = Guid.NewGuid()
        };

        _ticketServiceMock
            .Setup(s => s.Get(ticket.Id))
            .Returns(ticket);

        GetTicketResponse result = _controller.GetTicketById(ticket.Id.ToString());

        result.Should().NotBeNull();
        result.Should().BeOfType<GetTicketResponse>();
        result.Id.Should().Be(ticket.Id.ToString());
        result.Type.Should().Be(ticket.Type.ToString());
        result.Date.Should().Be(ticket.Date.ToString("yyyy-MM-dd"));
        result.EventId.Should().Be(ticket.EventId.ToString());
        result.QrId.Should().Be(ticket.QrId.ToString());
        result.VisitorId.Should().Be(visitorId.ToString());

        _ticketServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetTicketById_WhenEventIdIsNull_ShouldReturnResponseWithNullEventId()
    {
        var visitorId = Guid.NewGuid();
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 12, 15),
            Type = EntranceType.General,
            EventId = null,
            VisitorProfileId = visitorId,
            QrId = Guid.NewGuid()
        };

        _ticketServiceMock
            .Setup(s => s.Get(ticket.Id))
            .Returns(ticket);

        var result = _controller.GetTicketById(ticket.Id.ToString());

        result.Should().NotBeNull();
        result.Should().BeOfType<GetTicketResponse>();

        result.Id.Should().Be(ticket.Id.ToString());
        result.Type.Should().Be(ticket.Type.ToString());
        result.Date.Should().Be(ticket.Date.ToString("yyyy-MM-dd"));
        result.QrId.Should().Be(ticket.QrId.ToString());
        result.VisitorId.Should().Be(visitorId.ToString());

        result.EventId.Should().Be(string.Empty);

        _ticketServiceMock.VerifyAll();
    }
    #endregion

    #region Create
    [TestMethod]
    [TestCategory("Behaviour")]
    public void CreateTicket_WhenRequestIsValid_ShouldReturnCreateTicketResponse()
    {
        var visitorId = Guid.NewGuid().ToString();
        var eventId = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreateTicketRequest
        {
            VisitorId = visitorId,
            Type = "Event",
            EventId = eventId,
            Date = "2025-12-15"
        };

        var expectedArgs = request.ToArgs();

        _ticketServiceMock
            .Setup(s => s.Create(It.Is<TicketArgs>(a =>
                a.VisitorId == expectedArgs.VisitorId &&
                a.Type == expectedArgs.Type &&
                a.EventId == expectedArgs.EventId)))
            .Returns(returnId);

        var response = _controller.CreateTicket(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateTicketResponse>();
        response.Id.Should().Be(returnId.ToString());

        _ticketServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void CreateTicket_WhenEventIdIsEmpty_ShouldThrowArgumentException()
    {
        var request = new CreateTicketRequest
        {
            VisitorId = Guid.NewGuid().ToString(),
            Type = "General",
            EventId = string.Empty,
            Date = "2025-12-15"
        };

        Action act = () => _controller.CreateTicket(request);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.*");
    }

    #endregion

    #region GetAll
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAllTickets_ShouldReturnMappedList()
    {
        var visitor1 = Guid.NewGuid();
        var visitor2 = Guid.NewGuid();

        var ticket1 = new Ticket
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 12, 15),
            Type = EntranceType.General,
            EventId = null,
            VisitorProfileId = visitor1,
            QrId = Guid.NewGuid()
        };

        var ticket2 = new Ticket
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 12, 16),
            Type = EntranceType.Event,
            EventId = Guid.NewGuid(),
            VisitorProfileId = visitor2,
            QrId = Guid.NewGuid()
        };

        var tickets = new List<Ticket> { ticket1, ticket2 };

        _ticketServiceMock
            .Setup(s => s.GetAll())
            .Returns(tickets);

        var result = _controller.GetAllTickets();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Type.Should().Be(ticket1.Type.ToString());
        first.Date.Should().Be(ticket1.Date.ToString("yyyy-MM-dd"));
        first.VisitorId.Should().Be(visitor1.ToString());

        var second = result.Last();
        second.Type.Should().Be(ticket2.Type.ToString());
        second.EventId.Should().Be(ticket2.EventId.ToString());
        second.VisitorId.Should().Be(visitor2.ToString());

        _ticketServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAllTickets_WhenServiceReturnsEmpty_ShouldReturnEmptyList()
    {
        _ticketServiceMock
            .Setup(s => s.GetAll())
            .Returns([]);

        var result = _controller.GetAllTickets();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _ticketServiceMock.VerifyAll();
    }

    #endregion

    #region Delete
    [TestMethod]
    [TestCategory("Behaviour")]
    public void DeleteTicket_ShouldRemoveTicket_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _ticketServiceMock
            .Setup(s => s.Remove(id))
            .Verifiable();

        _controller.DeleteTicket(id.ToString());

        _ticketServiceMock.VerifyAll();
    }
    #endregion

    #region GetTicketByVisitorId
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetTicketsByVisitor_WhenNoTicketsExist_ShouldReturnEmptyList()
    {
        var visitorId = Guid.NewGuid();

        _ticketServiceMock
            .Setup(s => s.GetTicketsByVisitor(visitorId))
            .Returns([]);

        var result = _controller.GetTicketsByVisitor(visitorId.ToString());

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _ticketServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetTicketsByVisitor_WhenTicketsExist_ShouldReturnMappedList()
    {
        var visitorId = Guid.NewGuid();

        var t1 = new Ticket
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 12, 15),
            Type = EntranceType.General,
            EventId = null,
            VisitorProfileId = visitorId,
            QrId = Guid.NewGuid()
        };

        var t2 = new Ticket
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 12, 16),
            Type = EntranceType.Event,
            EventId = Guid.NewGuid(),
            VisitorProfileId = visitorId,
            QrId = Guid.NewGuid()
        };

        var tickets = new List<Ticket> { t1, t2 };

        _ticketServiceMock
            .Setup(s => s.GetTicketsByVisitor(visitorId))
            .Returns(tickets);

        var result = _controller.GetTicketsByVisitor(visitorId.ToString());

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Id.Should().Be(t1.Id.ToString());
        first.Type.Should().Be(t1.Type.ToString());
        first.Date.Should().Be(t1.Date.ToString("yyyy-MM-dd"));
        first.QrId.Should().Be(t1.QrId.ToString());
        first.VisitorId.Should().Be(visitorId.ToString());
        first.EventId.Should().Be(string.Empty);

        var second = result.Last();
        second.Id.Should().Be(t2.Id.ToString());
        second.Type.Should().Be(t2.Type.ToString());
        second.Date.Should().Be(t2.Date.ToString("yyyy-MM-dd"));
        second.QrId.Should().Be(t2.QrId.ToString());
        second.VisitorId.Should().Be(visitorId.ToString());
        second.EventId.Should().Be(t2.EventId.ToString());

        _ticketServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetTicketsByVisitor_WhenGuidIsInvalid_ShouldThrowFormatException()
    {
        const string invalidGuid = "not-a-guid";

        Action act = () => _controller.GetTicketsByVisitor(invalidGuid);

        act.Should()
            .Throw<FormatException>();
    }

    #endregion
}
