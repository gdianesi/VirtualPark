using FluentAssertions;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.BusinessLogic.Test.TypeIncidences.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("TypeIncidence")]
public class TypeIncidenceTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Visitor_WhenCreated_ShouldHaveNonEmptyId()
    {
        var typeIncidence = new TypeIncidence();
        typeIncidence.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var typeIncidence = new TypeIncidence { Type = "Repair" };
        typeIncidence.Type.Should().Be("Repair");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Setter_ReturnsAssignedValue()
    {
        var typeIncidence = new TypeIncidence();
        typeIncidence.Type = "Repair";
        typeIncidence.Type.Should().Be("Repair");
    }
    #endregion

}
