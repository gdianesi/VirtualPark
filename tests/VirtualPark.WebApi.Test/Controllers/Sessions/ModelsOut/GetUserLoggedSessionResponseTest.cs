using FluentAssertions;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Sessions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetUserLoggedSessionResponse")]
public class GetUserLoggedSessionResponseTest
{
    private static User BuildUser(
        Guid? id = null,
        Guid? visitorId = null,
        List<string>? roleNames = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Test",
            LastName = "User",
            Email = "test@test.com",
            VisitorProfileId = visitorId,
            Roles = (roleNames ?? ["Admin"])
                .Select(r => new Role { Id = Guid.NewGuid(), Name = r })
                .ToList()
        };
    }

    [TestMethod]
    public void Constructor_ShouldMapAllPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var roles = new List<string> { "Administrator", "Operator" };

        var user = BuildUser(
            id: id,
            visitorId: visitorId,
            roleNames: roles);

        var response = new GetUserLoggedSessionResponse(user);

        response.Id.Should().Be(id.ToString());
        response.VisitorId.Should().Be(visitorId.ToString());
        response.Roles.Should().BeEquivalentTo(roles);
    }
}
