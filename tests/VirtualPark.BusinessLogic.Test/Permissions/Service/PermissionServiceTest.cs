using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Permissions.Models;
using VirtualPark.BusinessLogic.Permissions.Service;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Permissions.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("Permission")]
public sealed class PermissionServiceTest
{
    private Mock<IRepository<Permission>> _permissionRepositoryMock = null!;
    private Mock<IRepository<Role>> _roleRepositoryMock = null!;
    private PermissionService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _permissionRepositoryMock = new Mock<IRepository<Permission>>();
        _roleRepositoryMock = new Mock<IRepository<Role>>();

        _service = new PermissionService(
            _roleRepositoryMock.Object,
            _permissionRepositoryMock.Object);
    }

    #region Create
    #region Success
    [TestMethod]
    [TestCategory("Service")]
    [TestCategory("Permission")]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldReturnPermissionId()
    {
        var role = new Role { Name = "Admin", Description = "Administrator role" };

        _roleRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(role);

        var args = new PermissionArgs("Can manage users", "user.manage", [role.Id]);

        Permission? captured = null;
        _permissionRepositoryMock
            .Setup(r => r.Add(It.IsAny<Permission>()))
            .Callback<Permission>(p => captured = p);

        var id = _service.Create(args);

        id.Should().NotBeEmpty();
        captured.Should().NotBeNull();
        captured!.Key.Should().Be("user.manage");
        captured.Roles.Should().ContainSingle(r => r.Id == role.Id);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Service")]
    [TestCategory("Permission")]
    [TestCategory("Behaviour")]
    public void Create_WhenRoleDoesNotExist_ShouldThrow()
    {
        var roleId = Guid.NewGuid();

        _roleRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Role?)null);

        var args = new PermissionArgs("Can manage users", "user.manage", [roleId]);

        Action act = () => _service.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Role with id {roleId} not found.");
    }
    #endregion
    #endregion

    [TestMethod]
    [TestCategory("Service")]
    [TestCategory("Permission")]
    [TestCategory("Behaviour")]
    public void Update_WhenRoleDoesNotExist_ShouldThrow()
    {
        var id = Guid.NewGuid();
        var existing = new Permission
        {
            Key = "user.view",
            Description = "View users",
            Roles = new List<Role>()
        };

        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns(existing);

        var roleId = Guid.NewGuid();

        _roleRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Role?)null);

        var args = new PermissionArgs("New description", "user.edit", new List<Guid> { roleId });

        Action act = () => _service.Update(id, args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Role with id {roleId} not found.");
    }
}
