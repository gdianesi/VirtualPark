using FluentAssertions;
using VirtualPark.BusinessLogic.VisitRegistrations.Modules;

namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Modules;

[TestClass]
[TestCategory("Models")]
[TestCategory("VisitRegistrationArgs")]
public class VisitRegistrationArgsTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var visitRegistrationArgs = new VisitRegistrationArgs("2025-09-30");
        visitRegistrationArgs.Date.Should().Be("2025-09-30");
    }
}
