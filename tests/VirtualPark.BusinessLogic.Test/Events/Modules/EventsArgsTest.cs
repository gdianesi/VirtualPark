using FluentAssertions;
using VirtualPark.BusinessLogic.Events.Models;

namespace VirtualPark.BusinessLogic.Test.Events.Modules;

[TestClass]
[TestCategory("Models")]
[TestCategory("EventsArgsTest")]
public class EventsArgsTest
{
    #region Success
    [TestMethod]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var eventsArgs = new EventsArgs("Halloween");
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
        var act = () => new EventsArgs(name);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid event name");
    }
    #endregion
}
