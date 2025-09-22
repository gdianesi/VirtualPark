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

    #region Password
    [TestMethod]
    [TestCategory("Behaviour")]
    public void PasswordHash_WhenAssigned_ShouldBeStored()
    {
        var visitor = new Visitor { PasswordHash = "8743b52063cd84097a65d" };

        visitor.PasswordHash.Should().Be("8743b52063cd84097a65d");
    }
    #endregion

    #region DateOfBirth
    [TestMethod]
    [TestCategory("Behaviour")]
    public void DateOfBirth_WhenAssigned_ShouldBeStored()
    {
        var dob = new DateTime(2000, 1, 1);
        var visitor = new Visitor { DateOfBirth = dob };

        visitor.DateOfBirth.Should().Be(dob);
    }
    #endregion

    #region Score
    [TestMethod]
    [TestCategory("Constructor")]
    public void Score_WhenVisitorIsCreated_ShouldBeZeroByDefault()
    {
        var visitor = new Visitor();

        visitor.Score.Should().Be(0);
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Constructor")]
    public void Membership_WhenVisitorIsCreated_ShouldBeStandardByDefault()
    {
        var visitor = new Visitor();

        visitor.Membership.Should().Be(Membership.Standard);
    }
    #endregion
}
