using FluentAssertions;
using VirtualPark.BusinessLogic.Visitors.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Test.VisitorsProfile.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("VisitorProfileArgsTest")]
public class VisitorProfileArgsTest
{
    #region DateOfBirth
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var visitorProfileArgs = new VisitorProfileArgs("2002-07-30", "Standard");
        visitorProfileArgs.DateOfBirth.Should().Be(new DateOnly(2002, 07, 30));
    }
    #endregion

    #region failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithInvalidDateFormat_ThrowsArgumentException()
    {
        var act = () => new VisitorProfileArgs("2002/07/30", "Standard");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Invalid date format*")
            .And.ParamName.Should().Be("dateOfBirth");
    }
    #endregion
    #endregion

    #region Membership
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var visitorProfileArgs = new VisitorProfileArgs("2002-07-30", "Standard");
        visitorProfileArgs.Membership.Should().Be(Membership.Standard);
    }
    #endregion

    #region failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithInvalidMembershipFormat_ThrowsArgumentException()
    {
        var act = () => new VisitorProfileArgs("2002-07-30", "Gold");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Invalid membership value*")
            .And.ParamName.Should().Be("membership");
    }
    #endregion
    #endregion
}
