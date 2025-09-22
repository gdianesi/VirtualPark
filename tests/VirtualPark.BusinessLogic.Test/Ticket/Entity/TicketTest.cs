using FluentAssertions;

namespace VirtualPark.BusinessLogic.Test.Ticket.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Ticket")]
public sealed class TicketTest
{
    #region ID
    [TestMethod]
    public void WhenTicketIsCreated_IdIsAssigned()
    {
        var ticket = new Tickets.Entity.Ticket();
        ticket.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
}
