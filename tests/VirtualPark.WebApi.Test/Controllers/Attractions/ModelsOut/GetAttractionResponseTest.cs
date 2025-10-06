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
        var attraction = new CreateAttractionResponse { Id = typeId.ToString() };
        attraction.TypeId.Should().Be(id.ToString());
    }
    #endregion
}
