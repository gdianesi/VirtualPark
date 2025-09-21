using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Test.VisitorsProfile.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitorProfile")]
public class VisitorProfileTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Visitor_WhenCreated_ShouldHaveNonEmptyId()
    {
        var visitorProfile = new VisitorProfile();
        visitorProfile.Id.Should().NotBe(System.Guid.Empty);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_ShouldBeGettable()
    {
        var visitorProfile = new VisitorProfile { DateOfBirth = new DateOnly(2002, 07, 30) };
        visitorProfile.DateOfBirth.Should().Be(new DateOnly(2002, 07, 30));
    }
}
