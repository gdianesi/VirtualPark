using FluentAssertions;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.WebApi.Controllers.Permissions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Permissions.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetPermissionResponse")]
public class GetPermissionResponseTest
{
    private static Permission BuildEntity(
        Guid? id = null,
        string? description = null,
        string? key = null,
        List<Guid>? roleIds = null)
    {
        return new Permission
        {
            Id = id ?? Guid.NewGuid(),
            Description = description ?? "Default description",
            Key = key ?? "Default.Key",
            Roles = (roleIds ?? [])
                .Select(r => new Role { Id = r, Name = "TestRole", Description = "desc" })
                .ToList()
        };
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetPermissionResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Description
    [TestMethod]
    public void Description_ShouldMapCorrectly()
    {
        var entity = BuildEntity(description: "Permite crear usuarios");

        var dto = new GetPermissionResponse(entity);

        dto.Description.Should().Be("Permite crear usuarios");
    }
    #endregion

    #region Key
    [TestMethod]
    public void Key_ShouldMapCorrectly()
    {
        var entity = BuildEntity(key: "User.Create");

        var dto = new GetPermissionResponse(entity);

        dto.Key.Should().Be("User.Create");
    }
    #endregion

    #region Roles
    [TestMethod]
    public void Roles_ShouldMapCorrectly()
    {
        var r1 = Guid.NewGuid();
        var r2 = Guid.NewGuid();

        var entity = BuildEntity(roleIds: [r1, r2]);

        var dto = new GetPermissionResponse(entity);

        dto.Roles.Should().BeEquivalentTo([r1.ToString(), r2.ToString()]);
    }
    #endregion
}
