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
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.Type.Should().Be("RollerCoaster");
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.Name.Should().Be("The Big Bang");
    }
    #endregion
    #region MinumAge

    [TestMethod]
    [TestCategory("Validation")]
    public void MiniumAge_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.MiniumAge.Should().Be(13);
    }
    #endregion
    #region Capacity
    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.Capacity.Should().Be(500);
    }
    #endregion
    #region Description

    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.Description.Should().Be("Description");
    }
    #endregion
    #region CurrentVisitor

    [TestMethod]
    [TestCategory("Validation")]
    public void CurrentVisitor_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.CurrentVisitor.Should().Be(50);
    }
    #endregion
    #region Available

    [TestMethod]
    [TestCategory("Validation")]
    public void Available_Getter_ReturnsAssignedValue()
    {
        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.Available.Should().BeTrue();
    }
    #endregion
    #region Events

    [TestMethod]
    [TestCategory("Validation")]
    public void Events_Getter_ReturnsAssignedValue()
    {
        var expectedEvents = new List<Guid>
        {
            new Guid("f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10"),
            new Guid("c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1")
        };

        var attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true",
            ["f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10", "c8a0b0ef-9a4d-46d9-a3a0-2e5d0e32d4b1"]);
        attractionArgs.Events.Should().NotBeNull().And.HaveCount(2).And.ContainInOrder(expectedEvents);
    }
    #endregion
}
