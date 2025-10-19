using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Tickets.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Tickets.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateTicketRequest")]
public class CreateTicketRequestTest
{
    [TestInitialize]
    public void Initialize()
    {
        var mockClock = new Mock<IClockAppService>();
        mockClock.Setup(c => c.Now()).Returns(new DateTime(2025, 10, 15));
        ValidationServices.ClockService = mockClock.Object;
    }
    #region VisitorId

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorId_Getter_ShouldReturnAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var request = new CreateTicketRequest { VisitorId = guid };
        request.VisitorId.Should().Be(guid);
    }

    #endregion

    #region Type

    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ShouldReturnAssignedValue()
    {
        var request = new CreateTicketRequest { Type = EntranceType.General.ToString() };
        request.Type.Should().Be("General");
    }

    #endregion

    #region EventId

    [TestMethod]
    [TestCategory("Validation")]
    public void EventId_Getter_ShouldReturnAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var request = new CreateTicketRequest { EventId = guid };
        request.EventId.Should().Be(guid);
    }

    #endregion

    #region Date

    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ShouldReturnAssignedValue()
    {
        var request = new CreateTicketRequest { Date = "2025-10-10" };
        request.Date.Should().Be("2025-10-10");
    }

    #endregion

    #region ToArgs

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_WhenAllFieldsAreValid_ShouldReturnTicketArgs()
    {
        var visitorId = Guid.NewGuid().ToString();
        var eventId = Guid.NewGuid().ToString();
        const string date = "2025-10-16";

        var request = new CreateTicketRequest { VisitorId = visitorId, Type = "Event", EventId = eventId, Date = date };

        TicketArgs result = request.ToArgs();

        result.Should().NotBeNull();
        result.Date.Should().Be(DateTime.Parse(date));
        result.Type.Should().Be(EntranceType.Event);
        result.VisitorId.Should().Be(Guid.Parse(visitorId));
        result.EventId.Should().Be(Guid.Parse(eventId));
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_WhenEventIdIsNull_ShouldReturnTicketArgsWithNullEventId()
    {
        var visitorId = Guid.NewGuid().ToString();
        const string date = "2025-10-16";

        var request = new CreateTicketRequest
        {
            VisitorId = visitorId,
            Type = "General",
            EventId = null,
            Date = date
        };

        TicketArgs result = request.ToArgs();

        result.Should().NotBeNull();
        result.Date.Should().Be(DateTime.Parse(date));
        result.Type.Should().Be(EntranceType.General);
        result.VisitorId.Should().Be(Guid.Parse(visitorId));
        result.EventId.Should().Be(null);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_WhenVisitorIdIsInvalid_ShouldThrowFormatException()
    {
        var request = new CreateTicketRequest
        {
            VisitorId = "invalid-guid",
            Type = "Event",
            EventId = null,
            Date = "2025-10-16"
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<FormatException>()
            .WithMessage("The value 'invalid-guid' is not a valid GUID.");
    }
    #endregion
}
