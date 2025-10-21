using FluentAssertions;
using VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Incidences.ModelsOut;
[TestClass]
public class CreateIncidenceResponseTest
{
    #region Id

    [TestMethod]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new CreateIncidenceResponse(id);
        response.Id.Should().Be(id);
    }
    #endregion
}
