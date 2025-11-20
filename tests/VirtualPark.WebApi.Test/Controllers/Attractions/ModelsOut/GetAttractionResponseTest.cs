using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsOut;

[TestClass]
[TestCategory("GetAttractionResponse")]
public class GetAttractionResponseTest
{
    private static Attraction BuildEntity(
        Guid? id = null,
        string? name = null,
        string? type = null,
        int? miniumAge = null,
        int? capacity = null,
        string? description = null,
        bool? available = null,
        List<Guid>? eventIds = null)
    {
        return new Attraction
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "Titanic",
            Type = Enum.Parse<AttractionType>(type ?? "RollerCoaster"),
            MiniumAge = miniumAge ?? 18,
            Capacity = capacity ?? 50,
            Description = description ?? "Family ride",
            Available = available ?? true,
            Events = eventIds?.Select(e => new Event { Id = e }).ToList() ?? []
        };
    }

    #region Id
    [TestMethod]
    public void GetAttractionResponse_Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetAttractionResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Name
    [TestMethod]
    public void GetAttractionResponse_Name_ShouldMapCorrectly()
    {
        var entity = BuildEntity(name: "Titanic");

        var dto = new GetAttractionResponse(entity);

        dto.Name.Should().Be("Titanic");
    }

    #endregion

    #region Type
    [TestMethod]
    public void GetAttractionResponse_Type_ShouldMapCorrectly()
    {
        var entity = BuildEntity(type: "Simulator");

        var dto = new GetAttractionResponse(entity);

        dto.Type.Should().Be("Simulator");
    }

    #endregion

    #region MiniumAge
    [TestMethod]
    public void GetAttractionResponse_MiniumAge_ShouldMapCorrectly()
    {
        var entity = BuildEntity(miniumAge: 12);

        var dto = new GetAttractionResponse(entity);

        dto.MiniumAge.Should().Be("12");
    }

    #endregion

    #region Capacity
    [TestMethod]
    public void GetAttractionResponse_Capacity_ShouldMapCorrectly()
    {
        var entity = BuildEntity(capacity: 80);

        var dto = new GetAttractionResponse(entity);

        dto.Capacity.Should().Be("80");
    }

    #endregion

    #region Description
    [TestMethod]
    public void GetAttractionResponse_Description_ShouldMapCorrectly()
    {
        var entity = BuildEntity(description: "High-speed ride");

        var dto = new GetAttractionResponse(entity);

        dto.Description.Should().Be("High-speed ride");
    }
    #endregion

    #region Available
    [TestMethod]
    public void GetAttractionResponse_Available_ShouldMapCorrectly()
    {
        var entity = BuildEntity(available: false);

        var dto = new GetAttractionResponse(entity);

        dto.Available.Should().Be("False");
    }
    #endregion

    #region EventIds
    [TestMethod]
    public void GetAttractionResponse_EventIds_ShouldMapCorrectly()
    {
        var e1 = Guid.NewGuid();
        var e2 = Guid.NewGuid();

        var entity = BuildEntity(eventIds: [e1, e2]);

        var dto = new GetAttractionResponse(entity);

        dto.EventIds.Should().BeEquivalentTo([e1.ToString(), e2.ToString()]);
    }
    #endregion
}
