using FluentAssertions;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;
using VirtualPark.WebApi.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.Test.TypeIncidences.ModelsOut;

public class CreateTypeIncidenceResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new CreateTypeIncidenceResponse(id);
        response.Id.Should().Be(id);
    }
    #endregion
}
