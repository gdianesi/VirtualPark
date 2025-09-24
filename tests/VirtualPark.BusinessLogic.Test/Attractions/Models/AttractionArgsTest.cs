using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions.Models;

namespace VirtualPark.BusinessLogic.Test.Attractions.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("Attractions")]
[TestCategory("AttractionsArgs")]
public class AttractionArgsTest
{
    #region Type
    [TestMethod]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster");
        attractionArgs.Type.Should().Be("RollerCoaster");
    }
    #endregion
}
