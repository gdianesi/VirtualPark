namespace VirtualPark.BusinessLogic.Test.Users.Models;

public class UserArgsTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_shouldBeGettable()
    {
        var userArgs = new UserArgs { Name = "Pepe" };
        userArgs.Name.Should().Be("Pepe");
    }
}
