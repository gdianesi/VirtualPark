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
    public void WhenAttractionIsCreated_IdIsAssigned()
    {
        Attraction attraction = new Attraction();
        attraction.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Type
    [TestMethod]
    public void Type_GetSet_Works()
    {
        Attraction attraction = new Attraction { Type = AttractionType.RollerCoaster };
        attraction.Type.Should().Be(AttractionType.RollerCoaster);
    }
    #endregion
}
