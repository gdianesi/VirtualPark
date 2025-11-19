using FluentAssertions;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Incidences.ModelsOut;

[TestClass]
public class GetIncidenceResponseTest
{
    private static Incidence BuildEntity(
        Guid? id = null,
        Guid? typeId = null,
        string? typeName = null,
        string? description = null,
        DateTime? start = null,
        DateTime? end = null,
        Guid? attractionId = null,
        bool? active = null)
    {
        return new Incidence
        {
            Id = id ?? Guid.NewGuid(),

            Type = new()
            {
                Id = typeId ?? Guid.NewGuid(),
                Type = typeName ?? "TestType"
            },
            TypeIncidenceId = typeId ?? Guid.NewGuid(),

            Description = description ?? "DefaultDesc",
            Start = start ?? new DateTime(2025, 10, 6, 10, 0, 0),
            End = end ?? new DateTime(2025, 10, 6, 11, 0, 0),

            Attraction = new() { Id = attractionId ?? Guid.NewGuid() },
            AttractionId = attractionId ?? Guid.NewGuid(),

            Active = active ?? true
        };
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);
        var dto = new GetIncidenceResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region TypeId
    [TestMethod]
    public void TypeId_ShouldMapCorrectly()
    {
        var typeId = Guid.NewGuid();
        var entity = BuildEntity(typeId: typeId);

        var dto = new GetIncidenceResponse(entity);

        dto.TypeId.Should().Be(typeId.ToString());
    }
    #endregion

    #region Description
    [TestMethod]
    public void Description_ShouldMapCorrectly()
    {
        var entity = BuildEntity(description: "Falla eléctrica");

        var dto = new GetIncidenceResponse(entity);

        dto.Description.Should().Be("Falla eléctrica");
    }
    #endregion

    #region Start
    [TestMethod]
    public void Start_ShouldMapCorrectly()
    {
        var start = new DateTime(2025, 10, 6, 10, 0, 0);
        var entity = BuildEntity(start: start);

        var dto = new GetIncidenceResponse(entity);

        dto.Start.Should().Be(start.ToString());
    }
    #endregion

    #region End
    [TestMethod]
    public void End_ShouldMapCorrectly()
    {
        var end = new DateTime(2025, 10, 6, 11, 30, 0);
        var entity = BuildEntity(end: end);

        var dto = new GetIncidenceResponse(entity);

        dto.End.Should().Be(end.ToString());
    }
    #endregion

    #region AttractionId
    [TestMethod]
    public void AttractionId_ShouldMapCorrectly()
    {
        var attrId = Guid.NewGuid();
        var entity = BuildEntity(attractionId: attrId);

        var dto = new GetIncidenceResponse(entity);

        dto.AttractionId.Should().Be(attrId.ToString());
    }
    #endregion

    #region Active
    [TestMethod]
    public void Active_ShouldMapCorrectly()
    {
        var entity = BuildEntity(active: false);

        var dto = new GetIncidenceResponse(entity);

        dto.Active.Should().Be("False");
    }
    #endregion
}
