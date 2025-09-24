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
    public void Name_Getter_ReturnsAssignedValue()
    {
        var eventsArgs = new EventsArgs("Halloween", "2002-07-30");
        eventsArgs.Name.Should().Be("Halloween");
    }
    #endregion

    #region Failure
    [TestMethod]
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
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var eventArgs = new EventsArgs("Halloween",  "2002-07-30");
        eventArgs.Date.Should().Be("2002-07-30");
    }
    #endregion
}
