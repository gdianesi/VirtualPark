using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Models;

namespace VirtualPark.BusinessLogic.Test.Tickets.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("TicketsArgs")]
public sealed class TicketArgsTest
{
    #region Constructor
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenValuesAreValid_ShouldCreateArgs()
    {
        var visitorId = Guid.NewGuid();
        const string eventId = "d85b1407-351d-4694-9392-03acc5870eb1";

        var args = new TicketArgs("2025-12-15", "Event", eventId, visitorId);

        args.Date.Should().Be(new DateOnly(2025, 12, 15));
        args.Type.Should().Be(EntranceType.Event);
        args.EventId.Should().Be(Guid.Parse(eventId));
        args.VisitorId.Should().Be(visitorId);
    }

    #endregion

    #region Date
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var args = new TicketArgs("2025-12-15", "General", "d85b1407-351d-4694-9392-03acc5870eb1", Guid.NewGuid());

        args.Date.Should().Be(new DateOnly(2025, 12, 15));
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithInvalidDateFormat_ThrowsArgumentException()
    {
        const string invalidDate = "15/12/2025";

        Action act = () =>
        {
            _ = new TicketArgs(invalidDate, "General", "d85b1407-351d-4694-9392-03acc5870eb1", Guid.NewGuid());
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid date format: {invalidDate}. Expected format is yyyy-MM-dd");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithPastDate_ThrowsArgumentException()
    {
        const string pastDate = "2000-01-01";

        Action act = () =>
        {
            var unused = new TicketArgs(pastDate, "General", "d85b1407-351d-4694-9392-03acc5870eb1", Guid.NewGuid());
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid date: {pastDate}. Date cannot be in the past");
    }
    #endregion
    #endregion

    #region EntranceType
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenTypeIsValid_ShouldParseEntranceType()
    {
        var args = new TicketArgs("2025-12-15", "Event", "d85b1407-351d-4694-9392-03acc5870eb1", Guid.NewGuid());

        args.Type.Should().Be(EntranceType.Event);
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenTypeIsInvalid_ShouldThrowArgumentException()
    {
        const string invalidType = "InvalidType";

        Action act = () =>
        {
            _ = new TicketArgs("2025-12-15", invalidType, "d85b1407-351d-4694-9392-03acc5870eb1", Guid.NewGuid());
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid entrance type value: {invalidType}");
    }
    #endregion
    #endregion

    #region Event
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenEventIdIsValid_ShouldAssignEventId()
    {
        const string eventId = "d85b1407-351d-4694-9392-03acc5870eb1";

        var args = new TicketArgs("2025-12-15", "General", eventId, Guid.NewGuid());

        args.EventId.Should().Be(eventId);
    }
    #endregion
    #endregion

    [TestMethod]
    [DataRow(" ")]
    [DataRow(null)]
    [TestCategory("Validation")]
    public void Constructor_WhenEventIdIsNullOrWhiteSpace_ShouldThrowArgumentException(string? value)
    {
        Action act = () =>
        {
            _ = new TicketArgs("2025-12-15", "General", value!, Guid.NewGuid());
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid cannot be null or empty.");
    }
}
