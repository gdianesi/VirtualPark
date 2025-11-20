using FluentAssertions;
using VirtualPark.BusinessLogic.Events.Entity;

namespace VirtualPark.BusinessLogic.Test.Events.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Event")]
public sealed class EventTest
{
    #region ID
    [TestMethod]
    public void WhenEventIsCreated_IdIsAssigned()
    {
        var newEvent = new Event();
        newEvent.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
    #region Name

    [TestMethod]
    public void Name_GetSet_Works()
    {
        var newEvent = new Event { Name = "Halloween" };
        newEvent.Name.Should().Be("Halloween");
    }
    #endregion
    #region Date
    [TestMethod]
    public void Date_GetSet_Works()
    {
        var newEvent = new Event { Date = DateTime.Now };
        newEvent.Date.Should().BeCloseTo(newEvent.Date, precision: TimeSpan.FromMilliseconds(10));
    }
    #endregion
    #region Capacity
    [TestMethod]
    public void Capacity_GetSet_Works()
    {
        var newEvent = new Event { Capacity = 100 };
        newEvent.Capacity.Should().Be(100);
    }
    #endregion
    #region Cost

    [TestMethod]
    public void Cost_GetSet_Works()
    {
        var newEvent = new Event { Cost = 250 };
        newEvent.Cost.Should().Be(250);
    }
    #endregion
    #region Attraction

    [TestMethod]
    public void WhenAttractionIsCreated_ListAttractionIsAssigned()
    {
        var newEvent = new Event();
        newEvent.Attractions.Should().NotBeNull();
    }

    [TestMethod]
    public void WhenEventIsCreated_ListAttractioonIsEmpty()
    {
        var newEvent = new Event();
        newEvent.Attractions.Should().BeEmpty();
    }
    #endregion
    #region Tickets
    [TestMethod]
    public void WhenEventIsCreated_TicketsListIsAssigned()
    {
        var newEvent = new Event();
        newEvent.Tickets.Should().NotBeNull();
    }

    [TestMethod]
    public void WhenEventIsCreated_TicketsListIsEmpty()
    {
        var newEvent = new Event();
        newEvent.Tickets.Should().BeEmpty();
    }
    #endregion
}
