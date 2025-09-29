using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
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
        var vp = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        var visitRegistrationArgs = new VisitRegistrationArgs("2025-09-30", vp);
        visitRegistrationArgs.Date.Should().Be(new DateOnly(2025, 09, 30));
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitRegistrationArgs_ShouldThrowArgumentException_WhenDateFormatIsInvalid()
    {
        var vp = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        var invalidDate = "2025/12/30";

        Action act = () =>
        {
            var visitRegistrationArgs = new VisitRegistrationArgs(invalidDate, vp);
        };

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid date format: {invalidDate}. Expected format is yyyy-MM-dd");
    }

    #endregion

    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_ok()
    {
        var vp = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        var args = new VisitRegistrationArgs("2025-09-30", vp);

        args.VisitorProfile.Should().NotBeNull();
        args.VisitorProfile.Should().BeSameAs(vp);
        args.VisitorProfile.DateOfBirth.Should().Be(new DateOnly(2002, 7, 30));
        args.VisitorProfile.Membership.Should().Be(Membership.Standard);
        args.VisitorProfile.Score.Should().Be(85);
    }
}
