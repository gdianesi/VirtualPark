using FluentAssertions;
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
        var expectedId = Guid.Parse("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010");
        var incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
        incidenceArgs.TypeIncidence.Should().Be(expectedId);
    }
    #endregion
    #region Description

    [TestMethod]
    public void Description_ShouldParseStringGuid_ToGuidProperty()
    {
        var incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
        incidenceArgs.Description.Should().Be("Description");
    }
    #endregion
    #region Start
    [TestMethod]
    public void Start_ShouldParseStringDateTime_ToDateTimeProperty()
    {
        var expected = new DateTime(2025, 9, 27, 15, 30, 0);
        var incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
        incidenceArgs.Start.Should().Be(expected);
    }
    #endregion
    #region End

    [TestMethod]
    public void End_ShouldParseStringDateTime_ToDateTimeProperty()
    {
        var expected = new DateTime(2025, 9, 27, 15, 30, 0);
        var incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
        incidenceArgs.End.Should().Be(expected);
    }
    #endregion
    #region Id
    [TestMethod]
    public void IdAttraction_ShouldParseStringGuid_ToGuidProperty()
    {
        var expectedId = Guid.Parse("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010");
        var incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
        incidenceArgs.AttractionId.Should().Be(expectedId);
    }
    #endregion
    #region Active

    [TestMethod]
    public void Active_ShouldParseStringDateTime_ToDateTimeProperty()
    {
        var incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
        incidenceArgs.Active.Should().BeTrue();
    }
    #endregion
}
