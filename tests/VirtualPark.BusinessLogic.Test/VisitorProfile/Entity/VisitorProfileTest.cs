namespace VirtualPark.BusinessLogic.Test.VisitorProfile.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitorProfile")]
public class VisitorProfileTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void User_createdShouldBeHaveId()
    {
        var visitorProfile = new VisitorProfile();
        visitorProfile.Id.Should().NotBe(Guid.Empty);
    }
}
