using FluentAssertions;
using VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Incidences.ModelsOut;

[TestClass]
public class GetIncidenceResponseTest
{
    private static GetIncidenceResponse Build(
        string id = "ID-1",
        string typeId = "TYPE-1",
        string description = "DESC",
        string start = "2025-10-06T10:00:00",
        string end = "2025-10-06T11:00:00",
        string attractionId = "ATTR-1",
        string active = "true")
        => new(id, typeId, description, start, end, attractionId, active);

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = Build(id: id);
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

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var value = "Falla técnica en atracción";
        var response = Build(description: value);
        response.Description.Should().Be(value);
    }
    #endregion

    #region Start
    [TestMethod]
    [TestCategory("Validation")]
    public void Start_Getter_ReturnsAssignedValue()
    {
        var value = "2025-10-06T10:00:00";
        var response = Build(start: value);
        response.Start.Should().Be(value);
    }
    #endregion

    #region End
    [TestMethod]
    [TestCategory("Validation")]
    public void End_Getter_ReturnsAssignedValue()
    {
        var value = "2025-10-06T11:00:00";
        var response = Build(end: value);
        response.End.Should().Be(value);
    }
    #endregion

    #region AttractionId
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionId_Getter_ReturnsAssignedValue()
    {
        var value = Guid.NewGuid().ToString();
        var response = Build(attractionId: value);
        response.AttractionId.Should().Be(value);
    }
    #endregion

    #region Active
    [TestMethod]
    [TestCategory("Validation")]
    public void Active_Getter_ReturnsAssignedValue()
    {
        var value = "true";
        var response = Build(active: value);
        response.Active.Should().Be(value);
    }
    #endregion
}
