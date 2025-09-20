using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions;
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

    #region Name
    [TestMethod]
    public void Name_GetSet_Works()
    {
        Attraction attraction = new Attraction { Name = "The Big Bang" };
        attraction.Name.Should().Be("The Big Bang");
    }
    #endregion
    #region MiniumAge

    [TestMethod]
    public void MiniumAge_GetSet_Works()
    {
        Attraction attraction = new Attraction { MiniumAge = 13 };
        attraction.MiniumAge.Should().Be(13);
    }
    #endregion
    #region Capacity

    [TestMethod]
    public void Capacity_GetSet_Works()
    {
        Attraction attraction = new Attraction { Capacity = 60 };
        attraction.Capacity.Should().Be(60);
    }
    #endregion
}
