using FluentAssertions;
using VirtualPark.BusinessLogic.Events.Models;

namespace VirtualPark.BusinessLogic.Test.Events.Models;

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
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };
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
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };

        var act = () => new EventsArgs(name, "2002-07-30", 50, 200, attractions);

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
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };
        var eventArgs = new EventsArgs("Halloween", "2025-12-30", 50, 200, attractions);

        eventArgs.Date.Should().Be(new DateOnly(2025, 12, 30));
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void EventsArgs_ShouldThrowArgumentException_WhenDateFormatIsInvalid()
    {
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };
        const string invalidDate = "2025/12/30";

        Action act = () =>
        {
            _ = new EventsArgs("Halloween", invalidDate, 50, 200, attractions);
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
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };

        const string invalidDate = "2002-07-30";

        Action act = () =>
        {
            _ = new EventsArgs("Halloween", invalidDate, 50, 200, attractions);
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
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };

        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 100, 200, attractions);
        eventsArgs.Capacity.Should().Be(100);
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithNegativeCapacity_ThrowsArgumentException()
    {
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };

        var act = () => new EventsArgs("Halloween", "2025-12-30", -10, 200, attractions);

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
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };

        var eventsArgs = new EventsArgs("Halloween", "2025-12-30", 100, 500, attractions);
        eventsArgs.Cost.Should().Be(500);
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithNegativeCost_ThrowsArgumentException()
    {
        var attractionId = Guid.NewGuid().ToString();
        var attractions = new List<string> { attractionId };
        var act = () => new EventsArgs("Halloween", "2025-12-30", 100, -200, attractions);

        act.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    #endregion
    #endregion

    #region Attractions
    #region Success
    [TestClass]
    [TestCategory("Models")]
    [TestCategory("EventsArgs")]
    public class EventsArgsAttractionsTest
    {
        [TestMethod]
        [TestCategory("Validation")]
        public void Constructor_WhenAttractionsAreValid_ShouldAssignGuidList()
        {
            var attractionId1 = Guid.NewGuid().ToString();
            var attractionId2 = Guid.NewGuid().ToString();
            var attractions = new List<string> { attractionId1, attractionId2 };

            var args = new EventsArgs("Halloween", "2025-12-30", 100, 200, attractions);

            args.AttractionIds.Should().HaveCount(2);
            args.AttractionIds.Should().Contain(Guid.Parse(attractionId1));
            args.AttractionIds.Should().Contain(Guid.Parse(attractionId2));
        }
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenAttractionsListIsNull_ShouldThrowArgumentException()
    {
        Action act = () =>
        {
            _ = new EventsArgs("Halloween", "2025-12-30", 100, 200, null!);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("List cannot be null.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenAttractionsListIsEmpty_ShouldThrowArgumentException()
    {
        var attractions = new List<string>();

        Action act = () =>
        {
            _ = new EventsArgs("Halloween", "2025-12-30", 100, 200, attractions);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("List cannot be empty.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenAttractionsListHasInvalidGuid_ShouldThrowFormatException()
    {
        var attractions = new List<string> { "not-a-guid" };

        Action act = () =>
        {
            _ = new EventsArgs("Halloween", "2025-12-30", 100, 200, attractions);
        };

        act.Should()
            .Throw<FormatException>()
            .WithMessage("The value 'not-a-guid' is not a valid GUID.");
    }
    #endregion
    #endregion
}
