using FluentAssertions;
using VirtualPark.WebApi.Controllers.Events.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Events.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateEventRequest")]
public class CreateEventRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ShouldReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Name = "Halloween Party" };
        request.Name.Should().Be("Halloween Party");
    }
    #endregion
}
