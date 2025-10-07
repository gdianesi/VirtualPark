using FluentAssertions;
using VirtualPark.WebApi.Controllers.Permissions.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Permissions.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreatePermissionRequest")]
public class CreatePermissionRequestTest
{
    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var createPermissionRequest = new CreatePermissionRequest { Description = "description" };
        createPermissionRequest.Description.Should().Be("description");
    }
    #endregion

    #region Key
    [TestMethod]
    [TestCategory("Validation")]
    public void Key_Getter_ReturnsAssignedValue()
    {
        var createPermissionRequest = new CreatePermissionRequest { Key = "key" };
        createPermissionRequest.Key.Should().Be("key");
    }
    #endregion

    #region RolesIds
    [TestMethod]
    [TestCategory("Validation")]
    public void Roles_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var createPermissionRequest = new CreatePermissionRequest { RolesIds = [guid] };
        createPermissionRequest.RolesIds.Should().Contain([guid]);
    }
    #endregion

    #region ToArgs
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapAllFields_AndConvertRolesToGuids()
    {
        var guid1 = Guid.NewGuid().ToString();
        var guid2 = Guid.NewGuid().ToString();

        var request = new CreatePermissionRequest
        {
            Description = "Manage Users",
            Key = "USERS_MANAGE",
            RolesIds = [guid1, guid2]
        };

        var result = request.ToArgs();

        result.Should().NotBeNull();
        result.Description.Should().Be("Manage Users");
        result.Key.Should().Be("USERS_MANAGE");
        result.Roles.Should().HaveCount(2);
        result.Roles.Should().Contain(Guid.Parse(guid1));
        result.Roles.Should().Contain(Guid.Parse(guid2));
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenDescriptionIsNullOrEmpty()
    {
        var request = new CreatePermissionRequest
        {
            Description = null,
            Key = "KEY",
            RolesIds = [Guid.NewGuid().ToString()]
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.*");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenKeyIsNullOrEmpty()
    {
        var request = new CreatePermissionRequest
        {
            Description = "Desc",
            Key = string.Empty,
            RolesIds = [Guid.NewGuid().ToString()]
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.*");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenRolesIdsIsNull()
    {
        var request = new CreatePermissionRequest
        {
            Description = "Desc",
            Key = "Key",
            RolesIds = null
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Role list can't be null");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenRolesIdsIsEmpty()
    {
        var request = new CreatePermissionRequest
        {
            Description = "Desc",
            Key = "Key",
            RolesIds = []
        };

        var act = request.ToArgs;

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Role list can't be null");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenRolesIdsContainInvalidGuid()
    {
        var request = new CreatePermissionRequest
        {
            Description = "Desc",
            Key = "Key",
            RolesIds = ["invalid-guid"]
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<FormatException>();
    }
    #endregion
    #endregion
}
