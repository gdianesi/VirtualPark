namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitRegistration")]
public sealed class VisitRegistrationTest
{
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitRegistrationIsCreated_ShouldAssignId()
    {
        var visit = new VisitRegistration();
        visit.Id.Should().NotBe(Guid.Empty);
    }
}
