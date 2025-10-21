using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsOut;

[TestClass]
[TestCategory("GetAttractionResponse")]
public class GetAttractionResponseTest
{
    private static GetAttractionResponse Build(
        string? id = null,
        string? name = null,
        string? type = null,
        string? miniumAge = null,
        string? capacity = null,
        string? description = null,
        List<string>? eventIds = null,
        string? available = null)
    {
        return new GetAttractionResponse(
            id: id ?? Guid.NewGuid().ToString(),
            name: name ?? "Titanic",
            type: type ?? "RollerCoaster",
            miniumAge: miniumAge ?? "18",
            capacity: capacity ?? "50",
            description: description ?? "Family ride",
            eventsId: eventIds ?? [],
            available: available ?? "true");
    }

    #region Id
    [TestMethod]
    public void GetAttractionResponse_IdProperty_ShouldMatchCtorValue()
    {
        var id = Guid.NewGuid().ToString();
        var attraction = Build(id: id);
        attraction.Id.Should().Be(id);
    }
    #endregion

    #region Name
    [TestMethod]
    public void GetAttractionResponse_NameProperty_ShouldMatchCtorValue()
    {
        var attraction = Build(name: "Titanic");
        attraction.Name.Should().Be("Titanic");
    }
    #endregion

    #region Type
    [TestMethod]
    public void GetAttractionResponse_TypeProperty_ShouldMatchCtorValue()
    {
        var attraction = Build(type: "Simulator");
        attraction.Type.Should().Be("Simulator");
    }
    #endregion

    #region MiniumAge
    [TestMethod]
    public void GetAttractionResponse_MiniumAgeProperty_ShouldMatchCtorValue()
    {
        var attraction = Build(miniumAge: "12");
        attraction.MiniumAge.Should().Be("12");
    }
    #endregion

    #region Capacity
    [TestMethod]
    public void GetAttractionResponse_CapacityProperty_ShouldMatchCtorValue()
    {
        var attraction = Build(capacity: "80");
        attraction.Capacity.Should().Be("80");
    }
    #endregion

    #region Description
    [TestMethod]
    public void GetAttractionResponse_DescriptionProperty_ShouldMatchCtorValue()
    {
        var attraction = Build(description: "High-speed ride");
        attraction.Description.Should().Be("High-speed ride");
    }
    #endregion

    #region Available
    [TestMethod]
    public void GetAttractionResponse_AvailableProperty_ShouldMatchCtorValue()
    {
        var attraction = Build(available: "false");
        attraction.Available.Should().Be("false");
    }
    #endregion

    #region EventIds
    [TestMethod]
    public void GetAttractionResponse_EventIdsProperty_ShouldMatchCtorValue()
    {
        var e1 = Guid.NewGuid().ToString();
        var e2 = Guid.NewGuid().ToString();

        var attraction = Build(eventIds: [e1, e2]);

        attraction.EventIds.Should().NotBeNull();
        attraction.EventIds!.Should().BeEquivalentTo([e1, e2]);
    }
    #endregion
}
