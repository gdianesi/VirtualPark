using FluentAssertions;
using VirtualPark.BusinessLogic.Users.Models;

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
        var userArgs = new UserArgs { Name = "Pepe" };
        userArgs.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var userArgs = new UserArgs { LastName = "Perez" };
        userArgs.LastName.Should().Be("Perez");
    }
    #endregion

    #region Email
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var userArgs = new UserArgs { Email = "pepeperez@gmail.com" };
        userArgs.Email.Should().Be("pepeperez@gmail.com");
    }
    #endregion
}
