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
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 50, 200, attractions);
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
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        Func<EventsArgs> act = () => new EventsArgs(name, "2002-07-30", 50, 200, attractions);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }

    #endregion

    #endregion

    #region Date

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var eventArgs = new EventsArgs("Halloween", "2025-12-30", 50, 200, attractions);
        eventArgs.Date.Should().Be(new DateOnly(2025, 12, 30));
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void EventsArgs_ShouldThrowArgumentException_WhenDateFormatIsInvalid()
    {
        var invalidDate = "2025/12/30";

        Action act = () =>
        {
            var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var eventsArgs = new EventsArgs("Halloween", invalidDate, 50, 200, attractions);
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
            var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var eventsArgs = new EventsArgs("Halloween", invalidDate, 50, 200, attractions);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid date: {invalidDate}. Date cannot be in the past");
    }

    #endregion

    #endregion

    #region Capacity

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 100, 200, attractions);
        eventsArgs.Capacity.Should().Be(100);
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithNegativeCapacity_ThrowsArgumentException()
    {
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        Func<EventsArgs> act = () => new EventsArgs("Halloween", "2025-12-30", -10, 200, attractions);

        act.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #endregion

    #region Cost

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void Cost_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 100, 500, attractions);
        eventsArgs.Cost.Should().Be(500);
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithNegativeCost_ThrowsArgumentException()
    {
        var attractions = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        Func<EventsArgs> act = () => new EventsArgs("Halloween", "2025-12-30", 100, -200, attractions);

        act.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #endregion

    #region Attracions

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Constructor_WhenAttractionIdsAreValid_ShouldInitializeSuccessfully()
    {
        var validIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var args = new EventsArgs("Test Event", "2025-12-31", 100, 500, validIds);

        args.AttractionIds.Should().BeEquivalentTo(validIds);
        args.Name.Should().Be("Test Event");
        args.Capacity.Should().Be(100);
        args.Cost.Should().Be(500);
    }

    #region Failure

    [TestMethod]
    [DataRow(null, DisplayName = "Null list")]
    [DataRow("empty", DisplayName = "Empty list")]
    [TestCategory("Validation")]
    public void Constructor_WhenAttractionIdsAreNullOrEmpty_ShouldThrowArgumentException(string caseType)
    {
        List<Guid>? ids = caseType switch
        {
            "empty" => [],
            _ => null
        };

        FluentActions.Invoking(() => new EventsArgs("Test", "2025-12-31", 100, 500, ids!))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("List cannot be null or empty");
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenAttractionIdsContainEmptyGuid_ShouldThrowArgumentException()
    {
        var invalidIds = new List<Guid> { Guid.NewGuid(), Guid.Empty };

        FluentActions.Invoking(() => new EventsArgs("Test", "2025-12-31", 100, 500, invalidIds))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("List contains invalid Guid");
    }

    #endregion

    #endregion
}
