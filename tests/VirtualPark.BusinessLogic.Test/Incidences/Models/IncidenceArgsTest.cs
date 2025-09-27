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
    [TestCategory("Incidences")]
    public void TypeIncidence_ShouldParseStringGuid_ToGuidProperty()
    {
        Guid expectedId = Guid.Parse("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010");

        var incidenceArgs = new IncidenceArgs("c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010");

        incidenceArgs.TypeIncidence.Should().Be(expectedId);
    }

    #endregion
}
