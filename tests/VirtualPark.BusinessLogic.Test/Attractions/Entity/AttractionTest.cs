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
        var attraction = new Attraction();
        attraction.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
    #region Type
    [TestMethod]
    public void Type_GetSet_Works()
    {
        var attraction = new Attraction { Type = AttractionType.RollerCoaster };
        attraction.Type.Should().Be(AttractionType.RollerCoaster);
    }
    #endregion
    #region Name
    [TestMethod]
    public void Name_GetSet_Works()
    {
        var attraction = new Attraction { Name = "The Big Bang" };
        attraction.Name.Should().Be("The Big Bang");
    }
    #endregion
    #region MiniumAge

    [TestMethod]
    public void MiniumAge_GetSet_Works()
    {
        var attraction = new Attraction { MiniumAge = 13 };
        attraction.MiniumAge.Should().Be(13);
    }
    #endregion
    #region Capacity

    [TestMethod]
    public void Capacity_GetSet_Works()
    {
        var attraction = new Attraction { Capacity = 60 };
        attraction.Capacity.Should().Be(60);
    }
    #endregion
    #region Description

    [TestMethod]
    public void Description_GetSet_Works()
    {
        var attraction = new Attraction { Description = "The largest roller coaster in the world." };
        attraction.Description.Should().Be("The largest roller coaster in the world.");
    }
    #endregion
    #region Events
    [TestMethod]
    public void WhenAttractionIsCreated_ListEventIsAssigned()
    {
        var attraction = new Attraction();
        attraction.Events.Should().NotBeNull();
    }

    [TestMethod]
    public void WhenAttractiomIsCreated_ListEventIsEmpty()
    {
        var attraction = new Attraction();
        attraction.Events.Should().BeEmpty();
    }
    #endregion
    #region CurrentVisitors

    [TestMethod]
    public void CurrentVisitors_GetSet_Works()
    {
        var attraction = new Attraction { CurrentVisitors = 40 };
        attraction.CurrentVisitors.Should().Be(40);
    }
    #endregion
    #region Available
    [TestMethod]
    public void Available_GetSet_Works()
    {
        var attraction = new Attraction { Available = false };
        attraction.Available.Should().BeFalse();
    }

    [TestMethod]
    public void WhenAttractionIsCreated_AvailableIsTrue()
    {
        var attraction = new Attraction();
        attraction.Available.Should().BeTrue();
    }
    #endregion
    #region VisitRegistration
    [TestMethod]
    public void WhenAttractionIsCreated_ListVisitRegistrationIsAssigned()
    {
        var attraction = new Attraction();
        attraction.VisitRegistrations.Should().NotBeNull();
    }

    [TestMethod]
    public void WhenAttractiomIsCreated_ListVisitRegistrationIsEmpty()
    {
        var attraction = new Attraction();
        attraction.VisitRegistrations.Should().BeEmpty();
    }
    #endregion
    #region IsDeleted

    [TestMethod]
    public void IsDeleted_GetSet_Works()
    {
        var attraction = new Attraction { IsDeleted = true };
        attraction.IsDeleted.Should().BeTrue();
    }

    [TestMethod]
    public void WhenAttractionIsCreated_IsDeletedIsFalse()
    {
        var attraction = new Attraction();
        attraction.IsDeleted.Should().BeFalse();
    }

    #endregion

}
