using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserResponse")]
public class GetUserResponseTest
{
    #region Id

    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(
            id,
            "pepe",
            "perez",
            "pepe@gmail.com",
            new List<string> { Guid.NewGuid().ToString() });
        response.Id.Should().Be(id);
    }

    #endregion

    #region Name

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id,
            "pepe",
            "perez",
            "pepe@gmail.com",
            new List<string> { Guid.NewGuid().ToString() });
        response.Name.Should().Be("pepe");
    }

    #endregion

    #region LastName

    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id,
            "pepe",
            "perez",
            "pepe@gmail.com",
            new List<string> { Guid.NewGuid().ToString() });
        response.LastName.Should().Be("perez");
    }

    #endregion

    #region Email

    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id,
            "pepe",
            "perez",
            "pepe@gmail.com",
            new List<string> { Guid.NewGuid().ToString() });
        response.Email.Should().Be("pepe@gmail.com");
    }

    #endregion

    #region Roles

    [TestMethod]
    [TestCategory("Validation")]
    public void Role_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var guid = Guid.NewGuid().ToString();
        var response = new GetUserResponse(id,
            "pepe",
            "perez",
            "pepe@gmail.com",
            new List<string> { guid });
        response.Roles.Should().Contain([guid]);
    }

    #endregion
}
