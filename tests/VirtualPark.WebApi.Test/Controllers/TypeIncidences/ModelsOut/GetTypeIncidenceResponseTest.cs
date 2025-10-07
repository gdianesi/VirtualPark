using FluentAssertions;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.TypeIncidences.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetTypeIncidenceResponse")]
public class GetTypeIncidenceResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetTypeIncidenceResponse(
            id,
            "type");
        response.Id.Should().Be(id);
    }
    #endregion

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetTypeIncidenceResponse(
            id,
            "type");
        response.Type.Should().Be("type");
    }
    #endregion

}
