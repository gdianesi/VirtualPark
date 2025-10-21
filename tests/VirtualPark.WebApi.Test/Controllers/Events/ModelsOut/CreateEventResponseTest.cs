using FluentAssertions;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Events.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("CreateEventResponse")]
public class CreateEventResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new CreateEventResponse(id);
        response.Id.Should().Be(id);
    }
    #endregion
}
