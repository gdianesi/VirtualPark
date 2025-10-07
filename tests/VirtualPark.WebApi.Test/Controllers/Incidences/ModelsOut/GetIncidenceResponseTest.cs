using FluentAssertions;
using VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Incidences.ModelsOut;
[TestClass]
public class GetIncidenceResponseTest
{

    private static GetIncidenceResponse Build(
        string id = "ID-1",
        string? typeId = "TYPE-1",
        string? description = "DESC",
        string? start = "2025-10-06T10:00:00",
        string? end = "2025-10-06T11:00:00",
        string? attractionId = "ATTR-1",
        string? active = "true")
        => new(id, typeId, description, start, end, attractionId, active);

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetIncidenceResponse(id);
        response.Id.Should().Be(id);
    }
    #endregion
    #region TypeId
    [TestMethod]
    [TestCategory("Validation")]
    public void TypeId_Getter_ReturnsAssignedValue()
    {
        var value = "Type123";
        var response = Build(typeId: value);
        response.TypeId.Should().Be(value);
    }
    #endregion
}
