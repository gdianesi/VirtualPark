using FluentAssertions;
using VirtualPark.BusinessLogic.AttractionsEvents.Entity;

namespace VirtualPark.BusinessLogic.Test.AttractionsEvents.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("AttractionEvent")]
public class AttractionEventTest
{
    #region AttractionId
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionEvent_GetterAttractionId_ReturnsAssignedValue()
    {
        var attractionId = Guid.NewGuid();
        var attractionEvent = new AttractionEvent { AttractionId = attractionId };

        attractionEvent.AttractionId.Should().Be(attractionId);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionEvent_SetterAttractionId_ReturnsAssignedValue()
    {
        var attractionId = Guid.NewGuid();
        var attractionEvent = new AttractionEvent();

        attractionEvent.AttractionId = attractionId;

        attractionEvent.AttractionId.Should().Be(attractionId);
    }
    #endregion
    #endregion

    #region EventId
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionEvent_GetterEventId_ReturnsAssignedValue()
    {
        var eventId = Guid.NewGuid();
        var attractionEvent = new AttractionEvent { EventId = eventId };

        attractionEvent.EventId.Should().Be(eventId);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionEvent_SetterEventId_ReturnsAssignedValue()
    {
        var eventId = Guid.NewGuid();
        var attractionEvent = new AttractionEvent();

        attractionEvent.EventId = eventId;

        attractionEvent.EventId.Should().Be(eventId);
    }
    #endregion
    #endregion
}
