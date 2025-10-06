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
    #region Description

    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var createIncidenceRequest = new CreateIncidenceRequest { Description = "Description" };
        createIncidenceRequest.Description.Should().Be("Description");
    }
    #endregion
    #region Start

    [TestMethod]
    [TestCategory("Validation")]
    public void Start_Getter_ReturnsAssignedValue()
    {
        var createIncidenceRequest = new CreateIncidenceRequest { Start = "2025-10-06T18:45:00" };
        createIncidenceRequest.Start.Should().Be("2025-10-06T18:45:00");
    }
    #endregion
}
