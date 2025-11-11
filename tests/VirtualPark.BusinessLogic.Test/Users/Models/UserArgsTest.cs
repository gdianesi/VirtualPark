using FluentAssertions;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Test.Users.Models;

[TestClass]
[TestCategory("Models")]
[TestCategory("UserArgs")]
public class UserArgsTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles);
        userArgs.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles);
        userArgs.LastName.Should().Be("Perez");
    }
    #endregion

    #region Email
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };
        var userArgs = new UserArgs("Pepe", "Perez", "pepeperez@gmail.com", "Password123.", roles);
        userArgs.Email.Should().Be("pepeperez@gmail.com");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException()
    {
        var invalidEmail = "pepe.perez";

        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };

        var act = () => new UserArgs("Pepe", "Perez", invalidEmail, "Password123!", roles);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"*{invalidEmail}*")
            .And.ParamName.Should().Be("email");
    }
    #endregion
    #endregion

    #region Password
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Password_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles);
        userArgs.Password.Should().Be("Password123.");
    }
    #endregion
    #endregion

    #region VisitorProfile
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Getter_ReturnsAssignedValue()
    {
        var visitorProfileArgs = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles) { VisitorProfile = visitorProfileArgs };
        userArgs.VisitorProfile.Should().Be(visitorProfileArgs);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Setter_ReturnsAssignedValue()
    {
        var visitorProfileArgs = new VisitorProfileArgs("2002-07-30", "Standard", "85");
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles) { VisitorProfile = visitorProfileArgs };
        userArgs.VisitorProfile = visitorProfileArgs;
        userArgs.VisitorProfile.Should().Be(visitorProfileArgs);
    }
    #endregion
    #endregion

    #region Roles
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void RolesIds_getter_ReturnsAssignedValue()
    {
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var roles = new List<string> { guid1.ToString(), guid2.ToString() };

        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles);

        userArgs.RolesIds.Should().HaveCount(2);
        userArgs.RolesIds.Should().Contain([guid1, guid2]);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void RolesIds_WithInvalidRoleId_ThrowsFormatException()
    {
        var roles = new List<string> { "guid" };

        var act = () => new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.", roles);

        act.Should()
            .Throw<FormatException>();
    }
    #endregion
    #endregion
}
