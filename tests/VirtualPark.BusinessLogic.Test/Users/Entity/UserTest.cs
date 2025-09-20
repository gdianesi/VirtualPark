using FluentAssertions;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Test.Users.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("User")]
public class UserTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void User_WhenCreated_ShouldHaveNonEmptyId()
    {
        var user = new User();
        user.Id.Should().NotBe(Guid.Empty);
    }
}
