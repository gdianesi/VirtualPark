using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.Tickets.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Tickets.Service;

[TestClass]
public class TicketServiceTest
{
    private Mock<IRepository<Ticket>> _ticketRepositoryMock = null!;
    private Mock<IRepository<VisitorProfile>> _visitorRepositoryMock = null!;
    private TicketService _service = null!;
    private VisitorProfileService _visitorProfileService = null!;

    [TestInitialize]
    public void Setup()
    {
        _ticketRepositoryMock = new Mock<IRepository<Ticket>>();
        _visitorRepositoryMock = new Mock<IRepository<VisitorProfile>>();
        _visitorProfileService = new VisitorProfileService(_visitorRepositoryMock.Object);
        _service = new TicketService(_ticketRepositoryMock.Object, _visitorProfileService);
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
            .Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitorProfile);

        var args = new TicketArgs("2025-12-15", "General", eventId, visitorId.ToString());

        var result = _service.Create(args);

        result.Should().NotBeNull();
        result.EventId.Should().Be(Guid.Parse(eventId));
        result.Visitor.Should().NotBeNull();
        result.Visitor.Id.Should().Be(visitorId);
        result.Type.Should().Be(EntranceType.General);

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

        _service.Remove(ticketId);

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

        Action act = () => _service.Remove(ticketId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Ticket with id {ticketId} not found.");
    }
    #endregion
    #endregion
}
