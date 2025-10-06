using FluentAssertions;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.VisitorsProfile.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetVisitorProfileResponse")]
public class GetVisitorProfileResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30");
        response.Id.Should().Be(id);
    }
    #endregion

    #region DateOfBirth
    [TestMethod]
    [TestCategory("Validation")]
    public void DateOfBirth_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetVisitorProfileResponse(
            id, "2002-07-30");
        response.DateOfBirth.Should().Be("2002-07-30");
    }
    #endregion
}
