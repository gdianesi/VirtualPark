using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public void Name_Getter_ReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Name = "Halloween Party" };
        request.Name.Should().Be("Halloween Party");
    }
    #endregion
}
