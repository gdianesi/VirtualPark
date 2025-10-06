using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsIn;

[TestClass]
[TestCategory("CreateAttractionRequest")]
public class CreateAttractionRequestTest
{
    #region Name

    [TestMethod]
    public void CreateAttractionRequest_NameProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new CreateAttractionRequest { Name = "Titanic" };
        attraction.Name.Should().Be("Titanic");
    }
    #endregion
    #region TypeId

    [TestMethod]
    public void CreateAttractionRequest_TypeIdProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var typeId = Guid.NewGuid();
        var attraction = new CreateAttractionRequest { TypeId = typeId.ToString() };
        attraction.TypeId.Should().Be(typeId.ToString());
    }
    #endregion
    #region MiniumAge
    [TestMethod]
    public void CreateAttractionRequest_MiniumAgeProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new CreateAttractionRequest { MiniumAge = "18" };
        attraction.MiniumAge.Should().Be("18");
    }
    #endregion
    #region Capacity

    [TestMethod]
    public void CreateAttractionRequest_CapacityProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new CreateAttractionRequest { Capacity = "50" };
        attraction.Capacity.Should().Be("50");
    }
    #endregion
    #region Description

    [TestMethod]
    public void CreateAttractionRequest_DescriptionProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new CreateAttractionRequest { Description = "Titanic" };
        attraction.Description.Should().Be("Titanic");
    }
    #endregion
    #region Events

    [TestMethod]
    public void CreateAttractionRequest_EventsProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var guid = Guid.NewGuid().ToString();
        var attraction = new CreateAttractionRequest() { EventIds = [guid] };
        attraction.EventIds.Should().Contain([guid]);
    }
    #endregion
    #region Available

    [TestMethod]
    public void CreateAttractionRequest_AvailableProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new CreateAttractionRequest { Available = "true" };
        attraction.Available.Should().Be("true");
    }
    #endregion
}
