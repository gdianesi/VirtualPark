using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions.Entity;
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

    #region AttractionId

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

    #region TypeIncidenceId
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void Incidence_GetterTypeIncidenceId_ReturnsAssignedValue()
    {
        var typeId = Guid.NewGuid();
        var incident = new Incidence() { TypeIncidenceId = typeId };

        incident.TypeIncidenceId.Should().Be(typeId);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void Incidence_SetterAttractionId_ReturnsAssignedValue()
    {
        var typeId = Guid.NewGuid();
        var incidence = new Incidence();

        incidence.TypeIncidenceId = typeId;

        incidence.TypeIncidenceId.Should().Be(typeId);
    }
    #endregion
    #endregion

    #region Attraction
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void Attraction_Getter_ReturnsAssignedValue()
    {
        var attraction = new Attraction();
        var incidence = new Incidence { Attraction = attraction };

        incidence.Attraction.Should().Be(attraction);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void Attraction_setter_ReturnsAssignedValue()
    {
        var attraction = new Attraction();
        var incidence = new Incidence();

        incidence.Attraction = attraction;

        incidence.Attraction.Should().Be(attraction);
    }
    #endregion
    #endregion
}
