using FluentAssertions;
using VirtualPark.BusinessLogic.Events.Models;

namespace VirtualPark.BusinessLogic.Test.Events.Modules;

[TestClass]
[TestCategory("Models")]
[TestCategory("EventsArgsTest")]
public class EventsArgsTest
{
    #region Name
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 50, 200);
        eventsArgs.Name.Should().Be("Halloween");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    public void Constructor_WithInvalidName_ThrowsArgumentException(string name)
    {
        var act = () => new EventsArgs(name, "2002-07-30", 50, 200);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid event name");
    }
    #endregion
    #endregion

    #region Date
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var eventArgs = new EventsArgs("Halloween", "2025-12-30", 50, 200);
        eventArgs.Date.Should().Be(new DateOnly(2025, 12, 30));
    }
    #endregion

    #region Failure
    [TestCategory("Validation")]
    [TestMethod]
    [TestCategory("Validation")]
    public void EventsArgs_ShouldThrowArgumentException_WhenDateFormatIsInvalid()
    {
        var invalidDate = "2025/12/30";

        Action act = () =>
        {
            var eventsArgs = new EventsArgs("Halloween", invalidDate, 50, 200);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid date format: {invalidDate}. Expected format is yyyy-MM-dd");
    }
    #endregion

    #region Failure
    [TestCategory("Validation")]
    [TestMethod]
    public void Constructor_WithPastDate_ThrowsArgumentException()
    {
        var invalidDate = "2002-07-30";

        Action act = () =>
        {
            var eventsArgs = new EventsArgs("Halloween", invalidDate, 50, 200);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid event date: {invalidDate}. Event date cannot be in the past");
    }
    #endregion
    #endregion

    #region Capacity
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 100, 200);
        eventsArgs.Capacity.Should().Be(100);
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithNegativeCapacity_ThrowsArgumentException()
    {
        var act = () => new EventsArgs("Halloween", "2025-12-30", -10, 200);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithParameterName("capacity");
    }
    #endregion
    #endregion

    #region Cost
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Cost_Getter_ReturnsAssignedValue()
    {
        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 100, 500);
        eventsArgs.Cost.Should().Be(500);
    }
    #endregion
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithNegativeCost_ThrowsArgumentException()
    {
        var act = () => new EventsArgs("Halloween", "2025-12-30", 100, -200);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithParameterName("cost");
    }
}
