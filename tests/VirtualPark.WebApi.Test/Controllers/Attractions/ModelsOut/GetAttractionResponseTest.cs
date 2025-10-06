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
        var attraction = new CreateAttractionResponse { Id = id.ToString() };
        attraction.Id.Should().Be(id.ToString());
    }
    #endregion
}
