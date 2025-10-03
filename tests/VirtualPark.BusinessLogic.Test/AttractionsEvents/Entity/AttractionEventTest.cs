using FluentAssertions;
using VirtualPark.BusinessLogic.AttractionsEvents.Entity;

namespace VirtualPark.BusinessLogic.Test.AttractionsEvents.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("AttractionEvent")]
public class AttractionEventTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionEvent_GetterAttractionId_ReturnsAssignedValue()
    {
        var attractionId = Guid.NewGuid();
        var attractionEvent = new AttractionEvent { AttractionId = attractionId };

        attractionEvent.AttractionId.Should().Be(attractionId);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionEvent_Setter()
    {
        var attractionId = Guid.NewGuid();
        var attractionEvent = new AttractionEvent();

        attractionEvent.AttractionId = attractionId;

        attractionEvent.AttractionId.Should().Be(attractionId);
    }
}
