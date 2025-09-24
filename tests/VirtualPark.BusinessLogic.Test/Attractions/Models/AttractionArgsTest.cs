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
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang");
        attractionArgs.Type.Should().Be("RollerCoaster");
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang");
        attractionArgs.Name.Should().Be("The Big Bang");
    }
    #endregion
    #region MinumAge

    [TestMethod]
    [TestCategory("Validation")]
    public void MiniumAge_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", 12);
        attractionArgs.MiniumAge.Should().Be(18);
    }
    #endregion
}
