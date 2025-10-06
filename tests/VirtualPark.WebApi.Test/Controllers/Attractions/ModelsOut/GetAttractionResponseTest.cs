using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsOut;

[TestClass]
[TestCategory("GetAttractionResponse")]
public class GetAttractionResponseTest
{
    #region Id

    [TestMethod]
    public void CreateAttractionResponse_IdProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var id = Guid.NewGuid();
        var attraction = new GetAttractionResponse { Id = id.ToString() };
        attraction.Id.Should().Be(id.ToString());
    }
    #endregion
    #region Name

    [TestMethod]
    public void CreateAttractionResponse_NameProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new GetAttractionResponse { Name = "Titanic" };
        attraction.Name.Should().Be("Titanic");
    }
    #endregion
    #region TypeId

    [TestMethod]
    public void CreateAttractionResponse_TypeIdProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var typeId = Guid.NewGuid();
        var attraction = new GetAttractionResponse { TypeId = typeId.ToString() };
        attraction.TypeId.Should().Be(typeId.ToString());
    }
    #endregion
    #region MiniumAge
    [TestMethod]
    public void CreateAttractionResponse_MiniumAgeProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new GetAttractionResponse { MiniumAge = "18" };
        attraction.MiniumAge.Should().Be("18");
    }
    #endregion
    #region Capacity

    [TestMethod]
    public void CreateAttractionResponse_CapacityProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new GetAttractionResponse { Capacity = "50" };
        attraction.Capacity.Should().Be("50");
    }
    #endregion
    #region Description

    [TestMethod]
    public void CreateAttractionResponse_DescriptionProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new GetAttractionResponse { Description = "Titanic" };
        attraction.Description.Should().Be("Titanic");
    }
    #endregion
    #region Events

    [TestMethod]
    public void CreateAttractionResponse_EventsProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var guid = Guid.NewGuid().ToString();
        var attraction = new GetAttractionResponse() { EventIds = [guid] };
        attraction.EventIds.Should().Contain([guid]);
    }
    #endregion
    #region Available

    [TestMethod]
    public void CreateAttractionResponse_AvailableProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new GetAttractionResponse { Available = "true" };
        attraction.Available.Should().Be("true");
    }
    #endregion
}
