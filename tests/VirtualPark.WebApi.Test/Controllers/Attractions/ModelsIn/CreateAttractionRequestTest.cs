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
        var attraction = new CreateAttractionRequestTest { TypeId = typeId.ToString() };
        attraction.TypeId.Should().Be(typeId.ToString());
    }
}
    #endregion
}
