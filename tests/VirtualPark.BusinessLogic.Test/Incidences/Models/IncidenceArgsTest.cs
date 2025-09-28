using FluentAssertions;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;

namespace VirtualPark.BusinessLogic.Test.Incidences.Models;

[TestClass]
[TestCategory("IncidenceArgs")]
public class IncidenceArgsTest
{
    #region TypeIncidence
    [TestMethod]
    public void TypeIncidence_ShouldParseStringGuid_ToGuidProperty()
    {
        Guid expectedId = Guid.Parse("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010");
        var incidenceArgs = new IncidenceArgs("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010", "Description", "27/09/2025 15:30");
        incidenceArgs.TypeIncidence.Should().Be(expectedId);
    }
    #endregion
    #region Description

    [TestMethod]
    public void Description_ShouldParseStringGuid_ToGuidProperty()
    {
        var incidenceArgs = new IncidenceArgs("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010", "Description", "27/09/2025 15:30");
        incidenceArgs.Description.Should().Be("Description");
    }
    #endregion
    #region Start

    [TestMethod]
    [TestCategory("Incidences")]
    public void Start_ShouldParseStringDateTime_ToDateTimeProperty()
    {
        var expected = new DateTime(2025, 9, 27, 15, 30, 0); 

        var incidenceArgs = new IncidenceArgs("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010", "Description", "27/09/2025 15:30");

        incidenceArgs.Start.Should().Be(expected);
    }
    #endregion
}
