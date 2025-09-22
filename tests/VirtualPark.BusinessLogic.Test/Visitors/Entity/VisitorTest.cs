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

    #region LastName
    [TestMethod]
    [TestCategory("Behaviour")]
    public void LastName_WhenAssigned_ShouldBeStored()
    {
        var visitor = new Visitor { LastName = "Doe" };

        visitor.LastName.Should().Be("Doe");
    }
    #endregion

    #region Email
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Email_WhenAssigned_ShouldBeStored()
    {
        var visitor = new Visitor { Email = "john.doe@mail.com" };

        visitor.Email.Should().Be("john.doe@mail.com");
    }
    #endregion
}
