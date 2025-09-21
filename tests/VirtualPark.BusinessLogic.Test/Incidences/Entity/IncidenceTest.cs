namespace VirtualPark.BusinessLogic.Test.Incidences.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Incidence")]
public class IncidenceTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Incidence_ShouldHaveNonEmptyId()
    {
        var incidence = new Incidence();
        incidence.Id.Should().NotBe(Guid.Empty);
    }
}
