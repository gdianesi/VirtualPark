using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

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
        VisitRegistration visit = new VisitRegistration();
        VisitRegistrationAttraction visitRegistrationAttraction = new VisitRegistrationAttraction(VisitRegistrationId = visit.Id);
        visitRegistrationAttraction.VisitRegistrationId.Should().Be(visit.Id);
    }
    #endregion
}
