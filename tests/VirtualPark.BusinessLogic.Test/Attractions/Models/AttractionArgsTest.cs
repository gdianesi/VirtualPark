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
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description");
        attractionArgs.Type.Should().Be("RollerCoaster");
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description");
        attractionArgs.Name.Should().Be("The Big Bang");
    }
    #endregion
    #region MinumAge

    [TestMethod]
    [TestCategory("Validation")]
    public void MiniumAge_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description");
        attractionArgs.MiniumAge.Should().Be(13);
    }
    #endregion
    #region Capacity
    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description");
        attractionArgs.Capacity.Should().Be(500);
    }
    #endregion
    #region Description

    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description");
        attractionArgs.Description.Should().Be("Description");
    }
    #endregion
}
