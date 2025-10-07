using FluentAssertions;
using VirtualPark.WebApi.Controllers.Roles.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Roles.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateRoleRequest")]
public class CreateRoleRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRoleRequest { Name = "Manager" };
        request.Name.Should().Be("Manager");
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRoleRequest { Description = "Manage users and content" };
        request.Description.Should().Be("Manage users and content");
    }
    #endregion

    #region PermissionIds (strings de Guid en el request)
    [TestMethod]
    [TestCategory("Validation")]
    public void PermissionIds_Getter_ReturnsAssignedValue()
    {
        var g1 = Guid.NewGuid().ToString();
        var g2 = Guid.NewGuid().ToString();

        var request = new CreateRoleRequest { PermissionsIds = [g1, g2] };

        request.PermissionsIds.Should().Contain([g1, g2]);
    }
    #endregion

    #region ToArgs
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapAllFields_WhenInputIsValid()
    {
        var g1 = Guid.NewGuid().ToString();
        var g2 = Guid.NewGuid().ToString();
        var g3 = Guid.NewGuid().ToString();

        var request = new CreateRoleRequest
        {
            Name = "Manager",
            Description = "Manage users and content",
            PermissionsIds = [g1, g2, g3]
        };

        var args = request.ToArgs(); // ¡invocar!

        args.Should().NotBeNull();
        args.Name.Should().Be("Manager");
        args.Description.Should().Be("Manage users and content");

        args.PermissionIds.Should().HaveCount(3);
        args.PermissionIds.Should().Contain(Guid.Parse(g1));
        args.PermissionIds.Should().Contain(Guid.Parse(g2));
        args.PermissionIds.Should().Contain(Guid.Parse(g3));
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenPermissionIdsIsEmpty()
    {
        var request = new CreateRoleRequest
        {
            Name = "Reporter",
            Description = "Read-only access",
            PermissionsIds = []
        };

        Action act = () => request.ToArgs();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Role list can't be null");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenPermissionIdsIsNull()
    {
        var request = new CreateRoleRequest
        {
            Name = "Reporter",
            Description = "Read-only access",
            PermissionsIds = null
        };

        Action act = () => request.ToArgs();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Role list can't be null");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenNameIsNullOrEmpty()
    {
        var okId = Guid.NewGuid().ToString();

        var nullName = new CreateRoleRequest { Name = null, Description = "desc", PermissionsIds = [okId] };
        var emptyName = new CreateRoleRequest { Name = string.Empty, Description = "desc", PermissionsIds = [okId] };

        Action act1 = () => nullName.ToArgs();
        Action act2 = () => emptyName.ToArgs();

        act1.Should().Throw<ArgumentException>();
        act2.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenDescriptionIsNullOrEmpty()
    {
        var okId = Guid.NewGuid().ToString();

        var nullDesc = new CreateRoleRequest { Name = "Role", Description = null, PermissionsIds = [okId] };
        var emptyDesc = new CreateRoleRequest { Name = "Role", Description = string.Empty, PermissionsIds = [okId] };

        Action act1 = () => nullDesc.ToArgs();
        Action act2 = () => emptyDesc.ToArgs();

        act1.Should().Throw<ArgumentException>();
        act2.Should().Throw<ArgumentException>();
    }
    #endregion
    #endregion
}
