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
    /*#region ToArgs
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapUserId_WhenGuidIsValid()
    {
        var userId = Guid.NewGuid();
        var request = new LogInSessionRequest { UserId = userId.ToString() };

        var args = request.ToArgs();

        args.UserId.Should().Be(userId);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenUserIdIsNull()
    {
        var request = new LogInSessionRequest { UserId = null };

        var act = request.ToArgs;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenUserIdIsEmpty()
    {
        var request = new LogInSessionRequest { UserId = string.Empty };

        var act = request.ToArgs;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenUserIdIsNotAGuid()
    {
        var request = new LogInSessionRequest { UserId = "not-a-guid" };

        var act = request.ToArgs;

        act.Should().Throw<FormatException>();
    }
    #endregion
    #endregion*/
}
