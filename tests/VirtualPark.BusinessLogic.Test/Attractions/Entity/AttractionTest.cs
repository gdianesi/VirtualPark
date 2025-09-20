using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions.Entity;

namespace VirtualPark.BusinessLogic.Test.Attractions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Attraction")]
public sealed class AttractionTest
{
    #region ID
    [TestMethod]
    public void WhemAttractionIsCreated_IdIsAssigned()
    {
        var attraction = new Attraction()
        attraction.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
}
