using FluentAssertions;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.TypeIncidences.ModelsOut;

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
