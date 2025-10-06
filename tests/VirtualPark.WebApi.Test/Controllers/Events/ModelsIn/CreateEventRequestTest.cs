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

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Date = "2025-10-31" };
        request.Date.Should().Be("2025-10-31");
    }
    #endregion

    #region Capacity
    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Capacity = "200" };
        request.Capacity.Should().Be("200");
    }
    #endregion
}
