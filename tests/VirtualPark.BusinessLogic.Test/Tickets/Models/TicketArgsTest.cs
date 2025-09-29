namespace VirtualPark.BusinessLogic.Test.Tickets.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("TicketsArgs")]
public sealed class TicketArgsTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenValuesAreValid_ShouldCreateArgs()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var args = new TicketArgs("2025-12-15", EntranceType.Event, eventId, visitorId);

        args.Date.Should().Be(new DateOnly(2025, 12, 15));
        args.Type.Should().Be(EntranceType.Event);
        args.EventId.Should().Be(eventId);
        args.VisitorId.Should().Be(visitorId);
    }
}
