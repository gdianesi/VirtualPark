using FluentAssertions;
using VirtualPark.BusinessLogic.Visitors.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitRegistration")]
public sealed class VisitRegistrationTest
{
    #region Id
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldAssignId()
    {
        var visit = new VisitRegistration();
        visit.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldInitializeDate()
    {
        var visit = new VisitRegistration();
        visit.Date.Should().NotBe(default);
    }
    #endregion

    #region Attractions
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldInitializeAttractionsList()
    {
        var visit = new VisitRegistration();
        visit.Attractions.Should().NotBeNull();
        visit.Attractions.Should().BeEmpty();
    }
    #endregion

    #region Visitor
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Visitor_WhenAssigned_ShouldBeStored()
    {
        var visitor = new Visitor();

        var visit = new VisitRegistration { Visitor = visitor };

        visit.Visitor.Should().Be(visitor);
    }
    #endregion
}
