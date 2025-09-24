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
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.");
        userArgs.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.");
        userArgs.LastName.Should().Be("Perez");
    }
    #endregion

    #region Email
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var userArgs = new UserArgs("Pepe", "Perez", "pepeperez@gmail.com", "Password123.");
        userArgs.Email.Should().Be("pepeperez@gmail.com");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException()
    {
        var invalidEmail = "pepe.perez";

        var act = () => new UserArgs("Pepe", "Perez", invalidEmail, "Password123!");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"*{invalidEmail}*")
            .And.ParamName.Should().Be("email");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithPasswordMissingSpecialCharacter_ThrowsArgumentException()
    {
        var invalidPassword = "Password123";

        var act = () => new UserArgs("Pepe", "Perez", "pepe.perez@mail.com", invalidPassword);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Password must be at least 8 characters long*")
            .And.ParamName.Should().Be("password");
    }
    #endregion
    #endregion

    #region Password
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Password_Getter_ReturnsAssignedValue()
    {
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.");
        userArgs.Password.Should().Be("Password123.");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithInvalidPassword_ThrowsArgumentException()
    {
        var invalidPassword = "pass";

        var act = () => new UserArgs("Pepe", "Perez", "pepe.perez@mail.com", invalidPassword);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Password must be at least 8 characters long*")
            .And.ParamName.Should().Be("password");
    }
    #endregion
    #endregion

    #region VisitorProfile
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Getter_ReturnsAssignedValue()
    {
        var visitorProfileArgs = new VisitorProfileArgs("2002-07-30", "Standard");
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.") { VisitorProfile = visitorProfileArgs };
        userArgs.VisitorProfile.Should().Be(visitorProfileArgs);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Setter_ReturnsAssignedValue()
    {
        var visitorProfileArgs = new VisitorProfileArgs("2002-07-30", "Standard");
        var userArgs = new UserArgs("Pepe", "Perez", "pepePerez@gmail.com", "Password123.");
        userArgs.VisitorProfile = visitorProfileArgs;
        userArgs.VisitorProfile.Should().Be(visitorProfileArgs);
    }
    #endregion
    #endregion
}
