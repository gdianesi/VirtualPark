namespace VirtualPark.BusinessLogic.Test.Tickets.Service;

public class TicketServiceTest
{
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldAddTicketAndReturnEntity()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid().ToString();
        var args = new TicketArgs("2025-12-15", "General", eventId, visitorId);

        var result = _service.Create(args);

        result.Should().NotBeNull();
        result.EventId.Should().Be(Guid.Parse(eventId));
        result.Visitor.Id.Should().Be(visitorId);
        result.Type.Should().Be(EntranceType.General);

        _repositoryMock.Verify(r => r.Add(It.IsAny<Ticket>()), Times.Once);
    }
}
