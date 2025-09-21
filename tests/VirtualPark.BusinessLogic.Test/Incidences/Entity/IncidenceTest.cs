using FluentAssertions;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.BusinessLogic.Test.Incidences.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Incidence")]
public class IncidenceTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Incidence_WhenCreated_ShouldHaveNonEmptyId()
    {
        var incidence = new Incidence();
        incidence.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var type = new TypeIncidence();
        var incidence = new Incidence { Type = type };
        incidence.Type.Should().Be(type);
    }
}
