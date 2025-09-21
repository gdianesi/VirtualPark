using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events;

namespace VirtualPark.BusinessLogic.Test.Events.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Events")]
public sealed class Event
{
    #region ID
    [TestMethod]
    public void WhenEventIsCreated_IdIsAssigned()
    {
        var newEvent = new Event();
        newEvent.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

}
