using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsRegistrationsAttractions.Entity;

namespace VirtualPark.BusinessLogic.Test.VisitsRegistrationsAttractions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitRegistrationAttraction")]
public class VisitRegistrationAttractionTest
{
    #region IdVisitRegistration

    [TestMethod]
    public void VisitRegistrationAttraction_GetterVisitRegistrationId_ReturnsAssignedValue()
    {
        var visit = new VisitRegistration();
        var visitRegistrationAttraction = new VisitRegistrationAttraction { VisitRegistrationId = visit.Id };
        visitRegistrationAttraction.VisitRegistrationId.Should().Be(visit.Id);
    }
    #endregion
    #region IdAttraction
    [TestMethod]
    public void VisitRegistrationAttraction_GetterAttractionId_ReturnsAssignedValue()
    {
        var attraction = new Attraction();
        var visitRegistrationAttraction = new VisitRegistrationAttraction { AttractionId = attraction.Id };
        visitRegistrationAttraction.AttractionId.Should().Be(attraction.Id);
    }
    #endregion
}
