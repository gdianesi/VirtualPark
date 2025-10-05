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

    [TestMethod]
    [TestCategory("Validation")]
    public void Score_Getter()
    {
        var createVisitorProfileRequest = new CreateVisitorProfileRequest { Score = "10" };
        createVisitorProfileRequest.Score.Should().Be("10");
    }
}
