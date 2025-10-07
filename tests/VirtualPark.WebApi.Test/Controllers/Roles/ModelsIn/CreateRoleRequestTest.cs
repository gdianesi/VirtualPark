using FluentAssertions;
using VirtualPark.WebApi.Controllers.Roles.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Roles.ModelsIn;

[TestClass]
public class CreateRoleRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRoleRequest { Name = "Admin" };
        request.Name.Should().Be("Admin");
    }
    #endregion
}
