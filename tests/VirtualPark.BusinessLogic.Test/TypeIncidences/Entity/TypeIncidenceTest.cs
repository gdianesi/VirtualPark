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

}
