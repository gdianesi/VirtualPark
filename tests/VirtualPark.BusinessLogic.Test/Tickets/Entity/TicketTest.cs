using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets.Entity;

namespace VirtualPark.BusinessLogic.Test.Tickets.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Ticket")]
public sealed class TicketTest
{
    #region ID
    [TestMethod]
    public void WhenTicketIsCreated_IdIsAssigned()
    {
        var ticket = new Ticket();
        ticket.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
    #region Date
    [TestMethod]
    public void Date_GetSet_Works()
    {
        var ticket = new Ticket{ Date = DateTime.Now };
        ticket.Date.Should().BeCloseTo(ticket.Date, precision: TimeSpan.FromMilliseconds(10));
    }
    #endregion
    #region Type
    public void Type_GetSet_Works()
    {
        var ticket = new Ticket { Type = EntranceType.General };
        ticket.Type().Should().Be(EntranceType.General);
    }
    #endregion
}
