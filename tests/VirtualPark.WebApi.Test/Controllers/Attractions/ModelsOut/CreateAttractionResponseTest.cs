using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsOut;

[TestClass]
[TestCategory("CreateAttractionRespone")]
public class CreateAttractionResponseTest
{
    #region Id

    [TestMethod]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var attraction = new CreateAttractionResponse(id.ToString());
        attraction.Id.Should().Be(id.ToString());
    }
    #endregion
}
