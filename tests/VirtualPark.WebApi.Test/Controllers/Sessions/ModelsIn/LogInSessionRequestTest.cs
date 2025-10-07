using FluentAssertions;
using VirtualPark.WebApi.Controllers.Sessions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("LogInSessionRequest")]
public class LogInSessionRequestTest
{
    #region UserId
    [TestMethod]
    [TestCategory("Validation")]
    public void UserId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var createSessionRequest = new LogInSessionRequest { UserId = id };
        createSessionRequest.UserId.Should().Be(id);
    }
    #endregion

    #region ToArgs
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
    #endregion
}
