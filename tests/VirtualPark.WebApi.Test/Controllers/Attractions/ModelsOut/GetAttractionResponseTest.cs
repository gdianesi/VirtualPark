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
}
