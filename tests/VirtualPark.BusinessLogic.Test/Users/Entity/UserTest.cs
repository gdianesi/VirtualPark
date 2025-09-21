using FluentAssertions;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Test.Users.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("User")]
public class UserTest
{
    #region  Id
    [TestMethod]
    [TestCategory("Validation")]
    public void User_WhenCreated_ShouldHaveNonEmptyId()
    {
        var user = new User();
        user.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region  Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var user = new User { Name = "Pepe" };
        user.Name.Should().Be("Pepe");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Setter_ReturnsAssignedValue()
    {
        var user = new User();
        user.Name = "Pepe";
        user.Name.Should().Be("Pepe");
    }
    #endregion

    #region  LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_getter_ReturnsAssignedValue()
    {
        var user = new User { LastName = "Perez" };
        user.LastName.Should().Be("Perez");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Setter_ReturnsAssignedValue()
    {
        var user = new User();
        user.LastName = "Perez";
        user.LastName.Should().Be("Perez");
    }
    #endregion

    #region  Email
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_getter_ReturnsAssignedValue()
    {
        var user = new User { Email = "pepitoperez@gmail.com" };
        user.Email.Should().Be("pepitoperez@gmail.com");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Setter_ReturnsAssignedValue()
    {
        var user = new User();
        user.Email = "pepitoperez@gmail.com";
        user.Email.Should().Be("pepitoperez@gmail.com");
    }
    #endregion

    #region  Password
    [TestMethod]
    [TestCategory("Validation")]
    public void Password_getter_ReturnsAssignedValue()
    {
        var user = new User { Password = "Password123." };
        user.Password.Should().Be("Password123.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Password_Setter_ReturnsAssignedValue()
    {
        var user = new User();
        user.Password = "Password123.";
        user.Password.Should().Be("Password123.");
    }
    #endregion

    #region  VisitorProfile
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Getter_ReturnsAssignedValue()
    {
        var visitorProfile = new VisitorProfile();
        var user = new User { VisitorProfile = visitorProfile };
        user.VisitorProfile.Should().Be(visitorProfile);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Setter_ReturnsAssignedValue()
    {
        var visitorProfile = new VisitorProfile();
        var user = new User();
        user.VisitorProfile = visitorProfile;
        user.VisitorProfile.Should().Be(visitorProfile);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var user = new User { VisitorProfileId = id };
        user.VisitorProfileId.Should().Be(id);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_Setter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var user = new User();
        user.VisitorProfileId = id;
        user.VisitorProfileId.Should().Be(id);
    }
    #endregion
}
