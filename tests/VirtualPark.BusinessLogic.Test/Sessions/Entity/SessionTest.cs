using FluentAssertions;
using VirtualPark.BusinessLogic.Sessions.Entity;
using VirtualPark.BusinessLogic.Users.Entity;

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

    #region  User
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void User_Getter_ReturnsAssignedValue()
    {
        var user = new User();
        var session = new Session { User = user };
        session.User.Should().Be(user);
    }
    #endregion
    #region Get
    [TestMethod]
    [TestCategory("Validation")]
    public void User_Setter_ReturnsAssignedValue()
    {
        var user = new User();
        var session = new Session();
        session.User = user;
        session.User.Should().Be(user);
    }
    #endregion
    #endregion
}
