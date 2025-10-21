using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions;
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
        var attraction = new CreateAttractionRequest { Type = typeId.ToString() };
        attraction.Type.Should().Be(typeId.ToString());
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
    #region Available

    [TestMethod]
    public void CreateAttractionRequest_AvailableProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var attraction = new CreateAttractionRequest { Available = "true" };
        attraction.Available.Should().Be("true");
    }
    #endregion
    #region ToArgs
    [TestMethod]
    public void ToArgs_ShouldReturnAttractionArgsWithSameValues()
    {
        var request = new CreateAttractionRequest
        {
            Name = "Roller Coaster",
            Type = "RollerCoaster",
            MiniumAge = "12",
            Capacity = "30",
            Description = "Fast ride",
            Available = "true"
        };

        var result = request.ToArgs();

        result.Should().NotBeNull();
        result.Name.Should().Be("Roller Coaster");
        result.Type.Should().Be(AttractionType.RollerCoaster);
        result.MiniumAge.Should().Be(12);
        result.Capacity.Should().Be(30);
        result.Description.Should().Be("Fast ride");
        result.Available.Should().BeTrue();
    }
    #endregion
}
