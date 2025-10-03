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

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var type = new TypeIncidence();
        var incidence = new Incidence { Type = type };
        incidence.Type.Should().Be(type);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Setter_ReturnsAssignedValue()
    {
        var type = new TypeIncidence();
        var incidence = new Incidence();
        incidence.Type = type;
        incidence.Type.Should().Be(type);
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_GetterSetter_ReturnsAssignedValue()
    {
        var incidence = new Incidence { Description = "Description" };
        incidence.Description.Should().Be("Description");
    }
    #endregion

    #region Start

    [TestMethod]
    [TestCategory("Validation")]
    public void Start_GetterSetter_ReturnsAssignedValue()
    {
        var incidence = new Incidence { Start = DateTime.Now };
        incidence.Start.Should().Be(incidence.Start);
    }
    #endregion

    #region End

    [TestMethod]
    [TestCategory("Validation")]
    public void End_GetterSetter_ReturnsAssignedValue()
    {
        var incidence = new Incidence { End = DateTime.Now };
        incidence.End.Should().Be(incidence.End);
    }
    #endregion

    #region Attraction

    [TestMethod]
    [TestCategory("Validation")]
    public void Attraction_GetterSetter_ReturnsAssignedValue()
    {
        var incidence = new Incidence { AttractionId = Guid.NewGuid() };
        incidence.AttractionId.Should().Be(incidence.AttractionId);
    }
    #endregion

    #region Active

    [TestMethod]
    [TestCategory("Validation")]
    public void Active_GetterSetter_ReturnsAssignedValue()
    {
        var incidence = new Incidence { Active = true };
        incidence.Active.Should().BeTrue();
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Incidence_Getter()
    {
        var typeId = Guid.NewGuid();
        var incident = new Incidence() { TypeIncidenceId = typeId };

        incident.TypeIncidenceId.Should().Be(typeId);
    }
}
