using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Test.VisitorsProfile.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("VisitorProfileArgsTest")]
public class VisitorProfileArgsTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_shouldBeGettable()
    {
        var visitorProfileArgs = new VisitorProfileArgs { DateOfBirth = new DateOnly(2002, 07, 30) };
        visitorProfileArgs.DateOfBirth.Should().Be(new DateOnly(2002, 07, 30));
    }
}
