using FluentAssertions;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserResponse")]
public class GetUserResponseTest
{
    private static User BuildUser(
        Guid? id = null,
        string? name = null,
        string? lastName = null,
        string? email = null,
        Guid? visitorId = null,
        List<Role>? roles = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "Pepe",
            LastName = lastName ?? "Perez",
            Email = email ?? "pepe@gmail.com",
            Roles = roles ?? [],
            VisitorProfileId = visitorId
        };
    }

    private static Role BuildRole(Guid? id = null, string? name = null)
    {
        return new Role
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "Admin",
            Description = string.Empty
        };
    }

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var user = BuildUser(id: id);

        var response = new GetUserResponse(user);

        response.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var user = BuildUser(name: "Pepe");

        var response = new GetUserResponse(user);

        response.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var user = BuildUser(lastName: "Perez");

        var response = new GetUserResponse(user);

        response.LastName.Should().Be("Perez");
    }
    #endregion

    #region Email
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var user = BuildUser(email: "test@mail.com");

        var response = new GetUserResponse(user);

        response.Email.Should().Be("test@mail.com");
    }
    #endregion

    #region Roles
    [TestMethod]
    [TestCategory("Validation")]
    public void Roles_Getter_ReturnsAssignedValue()
    {
        var r1 = BuildRole();
        var r2 = BuildRole();

        var user = BuildUser(roles: [r1, r2]);

        var response = new GetUserResponse(user);

        response.Roles.Should().BeEquivalentTo(
        [
            r1.Id.ToString(),
            r2.Id.ToString()
        ]);
    }
    #endregion

    #region VisitorProfileId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_Getter_ReturnsAssignedValue()
    {
        var visitorId = Guid.NewGuid();
        var user = BuildUser(visitorId: visitorId);

        var response = new GetUserResponse(user);

        response.VisitorProfileId.Should().Be(visitorId.ToString());
    }
    #endregion
}
