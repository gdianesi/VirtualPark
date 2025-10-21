using FluentAssertions;
using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("LogInSessionRequest")]
public class LogInSessionRequestTest
{
    #region Email
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var createSessionRequest = new LogInSessionRequest { Email = "email@gmail.com" };
        createSessionRequest.Email.Should().Be("email@gmail.com");
    }
    #endregion

    #region Password
    [TestMethod]
    [TestCategory("Validation")]
    public void Password_Getter_ReturnsAssignedValue()
    {
        var createSessionRequest = new LogInSessionRequest { Password = "Password!1" };
        createSessionRequest.Password.Should().Be("Password!1");
    }
    #endregion

    #region ToArgs
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapUserId_WhenGuidIsValid()
    {
        var request = new LogInSessionRequest { Email = "email@gmail.com", Password = "Password!1" };

        var args = request.ToArgs();

        args.Email.Should().Be("email@gmail.com");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenEmailNull()
    {
        var request = new LogInSessionRequest { Email = null, Password = "Password!1" };

        var act = request.ToArgs;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenPasswordIsEmpty()
    {
        var request = new LogInSessionRequest { Email = "email@gmail.com", Password = " " };

        var act = request.ToArgs;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion
}
