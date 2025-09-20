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

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var user = new User { Name = "Pepe" };
        user.Name.Should().Be("Pepe");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Setter_ReturnsAssignedValue()
    {
        var user = new User();
        user.Name = "Pepe";
        user.Name.Should().Be("Pepe");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_getter_ReturnsAssignedValue()
    {
        var user = new User { LastName = "Perez" };
        user.LastName.Should().Be("Perez");
    }
}
