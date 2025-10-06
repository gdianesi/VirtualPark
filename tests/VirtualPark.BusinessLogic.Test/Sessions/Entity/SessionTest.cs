using FluentAssertions;
using VirtualPark.BusinessLogic.Sessions.Entity;

namespace VirtualPark.BusinessLogic.Test.Sessions.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Session")]
public class SessionTest
{
    #region  Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Session_WhenCreated_ShouldHaveNonEmptyId()
    {
        var session = new Session();
        session.Id.Should().NotBe(Guid.Empty);
    }
    #endregion
}
