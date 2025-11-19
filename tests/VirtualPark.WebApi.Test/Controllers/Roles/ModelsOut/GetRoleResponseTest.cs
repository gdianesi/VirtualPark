using FluentAssertions;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Controllers.Roles.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Roles.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRoleResponse")]
public class GetRoleResponseTest
{
    private static Role BuildEntity(
        Guid? id = null,
        string? name = null,
        string? description = null,
        List<Guid>? permissionIds = null,
        List<Guid>? userIds = null)
    {
        return new Role
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "DefaultRole",
            Description = description ?? "DefaultDesc",

            Permissions = (permissionIds ?? [])
                .Select(pid => new Permission { Id = pid, Description = "d", Key = "k" })
                .ToList(),

            Users = (userIds ?? [])
                .Select(uid => new User { Id = uid, Name = "Test", LastName = "User", Email = "t@test.com" })
                .ToList()
        };
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetRoleResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Name
    [TestMethod]
    public void Name_ShouldMapCorrectly()
    {
        var entity = BuildEntity(name: "Admin");

        var dto = new GetRoleResponse(entity);

        dto.Name.Should().Be("Admin");
    }
    #endregion

    #region Description
    [TestMethod]
    public void Description_ShouldMapCorrectly()
    {
        var entity = BuildEntity(description: "Full access");

        var dto = new GetRoleResponse(entity);

        dto.Description.Should().Be("Full access");
    }
    #endregion

    #region PermissionIds
    [TestMethod]
    public void PermissionIds_ShouldMapCorrectly()
    {
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();

        var entity = BuildEntity(permissionIds: [p1, p2]);

        var dto = new GetRoleResponse(entity);

        dto.PermissionIds.Should().BeEquivalentTo([p1.ToString(), p2.ToString()]);
    }
    #endregion

    #region UsersIds
    [TestMethod]
    public void UsersIds_ShouldMapCorrectly()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();

        var entity = BuildEntity(userIds: [u1, u2]);

        var dto = new GetRoleResponse(entity);

        dto.UsersIds.Should().BeEquivalentTo([u1.ToString(), u2.ToString()]);
    }
    #endregion
}
