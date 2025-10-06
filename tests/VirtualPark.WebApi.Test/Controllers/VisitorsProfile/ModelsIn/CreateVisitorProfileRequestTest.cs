using FluentAssertions;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.VisitorsProfile.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateVisitorProfileRequest")]
public class CreateVisitorProfileRequestTest
{
    #region DateOfBirth
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var createVisitorProfileRequest = new CreateVisitorProfileRequest { DateOfBirth = "2002-07-30" };
        createVisitorProfileRequest.DateOfBirth.Should().Be("2002-07-30");
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var createVisitorProfileRequest = new CreateVisitorProfileRequest { Membership = "Standard" };
        createVisitorProfileRequest.Membership.Should().Be("Standard");
    }
    #endregion

    #region Score
    [TestMethod]
    [TestCategory("Validation")]
    public void Score_Getter_ReturnsAssignedValue()
    {
        var createVisitorProfileRequest = new CreateVisitorProfileRequest { Score = "10" };
        createVisitorProfileRequest.Score.Should().Be("10");
    }
    #endregion

    #region ToArgs
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldReturnVisitorProfileArgs_WithValidatedValues()
    {
        var request = new CreateVisitorProfileRequest
        {
            DateOfBirth = "2002-07-30",
            Membership = "Standard",
            Score = "90"
        };

        var result = request.ToArgs();

        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateOnly(2002, 07, 30));
        result.Membership.ToString().Should().Be("Standard");
        result.Score.Should().Be(90);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenAnyFieldIsEmpty()
    {
        var request = new CreateVisitorProfileRequest
        {
            DateOfBirth = string.Empty,
            Membership = string.Empty,
            Score = string.Empty
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>();
    }

    #endregion
}
