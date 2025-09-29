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

    #region Update
    #region Failure
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenRoleDoesNotExist_ShouldThrow()
    {
        var existing = new Permission
        {
            Key = "user.view",
            Description = "View users",
            Roles = []
        };

        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns(existing);

        var roleId = Guid.NewGuid();

        _roleRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Role?)null);

        var args = new PermissionArgs("New description", "user.edit", [roleId]);

        Action act = () => _service.Update(existing.Id, args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Role with id {roleId} not found.");
    }
    #endregion

    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenPermissionExists_ShouldApplyChangesAndPersist()
    {
        var existing = new Permission
        {
            Key = "user.view",
            Description = "Old description",
            Roles = []
        };

        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns(existing);

        var role = new Role { Name = "Admin", Description = "Administrator role" };

        _roleRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(role);

        var args = new PermissionArgs("New description", "user.edit", [role.Id]);

        _service.Update(existing.Id, args);

        existing.Description.Should().Be("New description");
        existing.Key.Should().Be("user.edit");
        existing.Roles.Should().ContainSingle(r => r.Id == role.Id);

        _permissionRepositoryMock.Verify(r => r.Update(existing), Times.Once);
    }
    #endregion
    #endregion

    #region Delete
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenPermissionExists_ShouldCallRepositoryRemove()
    {
        var permission = new Permission
        {
            Key = "user.delete",
            Description = "Allows deleting users"
        };

        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns(permission);

        _service.Remove(permission.Id);

        _permissionRepositoryMock.Verify(r => r.Remove(permission), Times.Once);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenPermissionDoesNotExist_ShouldThrow()
    {
        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Permission?)null);

        var id = Guid.NewGuid();

        Action act = () => _service.Remove(id);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Permission with id {id} not found.");
    }
    #endregion
    #endregion

    #region GetAll
    #region NoPredicate
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenNoPredicate_ShouldReturnAllPermissions()
    {
        var permissions = new List<Permission>
        {
            new Permission { Key = "user.view", Description = "View users" },
            new Permission { Key = "user.edit", Description = "Edit users" }
        };

        _permissionRepositoryMock
            .Setup(r => r.GetAll(null))
            .Returns(permissions);

        var result = _service.GetAll();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Key == "user.view");
        result.Should().Contain(p => p.Key == "user.edit");
    }
    #endregion

    #region Predicate
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenPredicateProvided_ShouldReturnFilteredPermissions()
    {
        var permissions = new List<Permission>
        {
            new Permission { Key = "user.view", Description = "View users" },
            new Permission { Key = "user.edit", Description = "Edit users" }
        };

        _permissionRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns<Expression<Func<Permission, bool>>>(expr => permissions.Where(expr.Compile()).ToList());

        var result = _service.GetAll(p => p.Key.Contains("edit"));

        result.Should().HaveCount(1);
        result[0].Key.Should().Be("user.edit");
    }
    #endregion
    #endregion

    #region GetById
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetById_WhenPermissionExists_ShouldReturnPermission()
    {
        var permission = new Permission { Key = "user.view", Description = "View users" };

        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns(permission);

        var result = _service.GetById(permission.Id);

        result.Should().NotBeNull();
        result?.Id.Should().Be(permission.Id);
        result?.Key.Should().Be("user.view");
    }
    #endregion
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetById_WhenPermissionDoesNotExist_ShouldReturnNull()
    {
        _permissionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Permission?)null);

        var id = Guid.NewGuid();

        var result = _service.GetById(id);

        result.Should().BeNull();
    }
    #endregion
}
