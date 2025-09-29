using FluentAssertions;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;

namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("VisitRegistrationArgs")]
public class VisitRegistrationArgsTest
{
    #region Date

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var visitRegistrationArgs = new VisitRegistrationArgs("2025-09-30");
        visitRegistrationArgs.Date.Should().Be(new DateOnly(2025, 09, 30));
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitRegistrationArgs_ShouldThrowArgumentException_WhenDateFormatIsInvalid()
    {
        var invalidDate = "2025/12/30";

        Action act = () =>
        {
            var visitRegistrationArgs = new VisitRegistrationArgs(invalidDate);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid date format: {invalidDate}. Expected format is yyyy-MM-dd");
    }

    #endregion

    #endregion
}
