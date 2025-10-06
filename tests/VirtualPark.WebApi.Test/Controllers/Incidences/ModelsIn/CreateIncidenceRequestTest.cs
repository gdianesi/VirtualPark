using FluentAssertions;
using VirtualPark.WebApi.Controllers.Incidences.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Incidences.ModelsIn;

[TestClass]
public class CreateIncidenceRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var typeId = Guid.NewGuid().ToString();
        var createIncidenceRequest = new CreateIncidenceRequest { TypeId = typeId };
        createIncidenceRequest.TypeId.Should().Be(typeId);
    }
    #endregion
}
