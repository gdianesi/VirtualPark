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
        var eventsArgs = new EventsArgs("Halloween", "2002-07-30");
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
        var act = () => new EventsArgs(name, "2002-07-30");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid event name");
    }
    #endregion
    #endregion

    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var eventArgs = new EventsArgs("Halloween",  "2002-07-30");
        eventArgs.Date.Should().Be(new DateOnly(2002, 07, 30));
    }
    #endregion

    #region Failure
    [TestCategory("Validation")]
    [TestMethod]
    public void Constructor_WithInvalidDateFormat_ThrowsArgumentException()
    {
        var act = () => new EventsArgs("Halloween", "2002/07/30");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid event date format. Expected yyyy-MM-dd");
    }
    #endregion

    [TestCategory("Validation")]
    [TestMethod]
    public void Constructor_WithInvalidDate_ThrowsArgumentException()
    {
        var act = () => new EventsArgs("Halloween", "2002-07-30");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Date cannot be in the past or future");
    }
}
