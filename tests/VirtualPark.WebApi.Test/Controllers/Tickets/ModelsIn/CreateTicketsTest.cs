using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.WebApi.Controllers.Tickets.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Tickets.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateTicketRequest")]
public class CreateTicketRequestTest
{
    #region VisitorId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorId_Getter_ShouldReturnAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var request = new CreateTicketRequest(guid, EntranceType.Event.ToString(), Guid.NewGuid().ToString(), "2025-10-10");
        request.VisitorId.Should().Be(guid);
    }
    #endregion

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ShouldReturnAssignedValue()
    {
        var request = new CreateTicketRequest(Guid.NewGuid().ToString(), EntranceType.General.ToString(), Guid.NewGuid().ToString(), "2025-10-10");
        request.Type.Should().Be("General");
    }
    #endregion

    #region EventId
    [TestMethod]
    [TestCategory("Validation")]
    public void EventId_Getter_ShouldReturnAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var request = new CreateTicketRequest(Guid.NewGuid().ToString(), EntranceType.General.ToString(), guid, "2025-10-10" );
        request.EventId.Should().Be(guid);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ShouldReturnAssignedValue()
    {
        var request = new CreateTicketRequest(Guid.NewGuid().ToString(), EntranceType.General.ToString(), Guid.NewGuid().ToString(), "2025-10-10");
        request.Date.Should().Be("2025-10-10");
    }
    #endregion

    #region ToArgs
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldReturnTicketArgs_WithValidatedValues()
    {
        var visitorId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var request = new CreateTicketRequest(Guid.NewGuid().ToString(), EntranceType.General.ToString(), Guid.NewGuid().ToString(), "2025-10-10");

        var result = request.ToArgs();

        result.VisitorId.Should().Be(visitorId);
        result.Type.ToString().Should().Be("Event");
        result.EventId.Should().Be(eventId);
    }
    #endregion
}
