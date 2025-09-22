using FluentAssertions;
using VirtualPark.BusinessLogic.Visitors.Entity;
namespace VirtualPark.BusinessLogic.Test.Visitors.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Visitor")]
public class VisitorTest
{
    #region Id
    [TestMethod]
    [TestCategory("Constructor")]
    public void Id_WhenVisitorIsCreated_ShouldNotBeEmpty()
    {
        var visitor = new Visitor();

        visitor.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Name_WhenAssigned_ShouldBeStored()
    {
        var visitor = new Visitor { Name = "John" };

        visitor.Name.Should().Be("John");
    }
    #endregion
}
