using FluentAssertions;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("LogInSessionResponse")]
public class LogInSessionResponseTest
{
    #region Token
    [TestMethod]
    [TestCategory("Validation")]
    public void Token_Getter_ReturnsAssignedValue()
    {
        var token = Guid.NewGuid().ToString();
        var response = new LogInSessionResponse(token);
        response.Token.Should().Be(token);
    }
    #endregion
}
